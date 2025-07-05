using AutoMapper;
using EcommerceAPI.Data.UnitOfWork;
using EcommerceAPI.Domain;
using EcommerceAPI.DTOs;
using EcommerceAPI.DTOs.GenericResponse;
using EcommerceAPI.Services.IServices;
using EcommerceAPI.Utilities;
using EcommerceAPI.Utilities.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAPI.Services.Services
{
    public class OrderServices : IOrderServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthenticationServices _authenticationServices;
        private readonly IShoppingCartServices _shoppingCartServices;

        public OrderServices(IUnitOfWork unitOfWork, IAuthenticationServices authenticationServices, IShoppingCartServices shoppingCartServices)
        {
            _unitOfWork = unitOfWork;
            _authenticationServices = authenticationServices;
            _shoppingCartServices = shoppingCartServices;
        }

        public async Task<string> CreateOrder(ShippingAddressDTO payload, string userId, string paymentIntentId)
        {
            if (payload == null) throw new ArgumentNullException("You must provide valid shipping address data.");

            if (string.IsNullOrEmpty(userId)) throw new UnauthorizedAccessException("User not authenticated.");

            // Validate user existence
            var user = await _authenticationServices.GetUserByIdAsync(userId);
            if (user == null) throw new UnauthorizedAccessException("User not authenticated.");

            // Get carts and check if there are any products in the shopping cart
            var carts = (await _shoppingCartServices.GetShoppingCartsByUserId(userId));
            if (carts?.Count() == 0) throw new ApiException(HttpStatusCode.BadRequest, message: "You cannot create an order without any products in your shopping cart.");

            var paymentIntentService = new PaymentIntentService();
            var paymentIntent = await paymentIntentService.GetAsync(paymentIntentId);

            if (paymentIntent.Status != "succeeded") throw new InvalidOperationException("Payment was not successful.");

            // Create a new OrderHeader object
            var orderHeader = new OrderHeader
            {
                // If user paid with online payment, then order status will be accepted by default. Admin can change it later and refund the money.
                OrderStatus = OrdersStatus.Accepted.ToString(),
                ApplicationUserId = user.Id,
                OrderAmount = Convert.ToDecimal(paymentIntent.Amount) / 100,
                Currency = paymentIntent.Currency,
                PaymentStatus = PaymentStatus.Paid.ToString(),
                PaymentType = PaymentType.OnlinePayment.ToString(),
                PaymentIntentId = paymentIntent.Id,
            };

            orderHeader.OrderAddress = new OrderAddress
            {
                FullName = payload.FullName,
                Phone = payload.Phone,
                Email = payload.Email,
                HouseName = payload.HouseName,
                RoadNumber = payload.RoadNumber,
                City = payload.City,
                Country = payload.Country,
                State = payload.State,
                PostCode = payload.PostCode,
                Province = payload.Province,
                AddressLine1 = payload.AddressLine1,
                AddressLine2 = payload.AddressLine2,
                AlternatePhone = payload.AlternatePhone,
                LandMark = payload.LandMark
            };

            orderHeader.OrderDetails = carts.Select(cart => new OrderDetail
            {
                ProductId = cart.ProductId,
                Quantity = cart.Count,
                Price = cart.Product.Price,
                OrderHeaderId = orderHeader.Id,
                CreatedAt = orderHeader.OrderDate
            }).ToList();

            await _unitOfWork.GenericRepository<OrderHeader>().AddAsync(orderHeader);

            // Remove products from shopping cart after order creation
            await _shoppingCartServices.RemoveProductsFromShoppingCart(productIDs: carts.Select(c => c.ProductId).ToList(), userId: userId);
            await _unitOfWork.SaveAsync();
            return orderHeader.Id;
        }

        public async Task CancelOrder(string orderId, string? userId = null)
        {
            var orderToCancel = await this.GetOrder(orderId, userId);
            if (orderToCancel == null) return;
            orderToCancel.OrderStatus = OrdersStatus.Canceled.ToString();
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<OrderHeader>> GetAllOrders(string? userId = null, OrdersStatus? orderStatus = null, PaymentStatus? paymentStatus = null, PaymentType? paymentType = null, SortBy? sortBy = SortBy.DateDESC)
        {
            // filter logic based on the provided parameters
            Expression<Func<OrderHeader, bool>> filter = o =>
                (userId == null || o.ApplicationUserId == userId)
                && (!orderStatus.HasValue || o.OrderStatus == orderStatus.ToString())
                && (!paymentStatus.HasValue || o.PaymentStatus == paymentStatus.ToString())
                && (!paymentType.HasValue || o.PaymentType == paymentType.ToString());

            // sorting logic based on the provided SortBy enum
            Func<IQueryable<OrderHeader>, IOrderedQueryable<OrderHeader>> orderBy = sortBy switch
            {
                SortBy.DateASC => q => q.OrderBy(o => o.OrderDate),
                SortBy.DateDESC => q => q.OrderByDescending(o => o.OrderDate),

                SortBy.AmountASC => q => q.OrderBy(o => o.OrderAmount),
                SortBy.AmountDESC => q => q.OrderByDescending(o => o.OrderAmount),

                _ => q => q.OrderByDescending(o => o.OrderDate)   // fallback
            };

            const string includes = "OrderDetails, OrderAddress, OrderDetails.Product, ApplicationUser";

            IEnumerable<OrderHeader> orders = new List<OrderHeader>();

            if (userId is not null)
            {
                // For specific user panel uses
                return await _unitOfWork.GenericRepository<OrderHeader>().GetAllAsync(predicate: o => o.ApplicationUserId == userId, includeProperties: includes, orderBy: orderBy);
            }
            // For admin panel uses
            return await _unitOfWork.GenericRepository<OrderHeader>().GetAllAsync(predicate: filter, includeProperties: includes, orderBy: orderBy);
        }

        public async Task<OrderHeader> GetOrder(string orderId, string? userId)
        {
            const string includes = "OrderDetails, OrderAddress, OrderDetails.Product, ApplicationUser";
            if (string.IsNullOrEmpty(userId))
            {
                // For admin panel uses
                return await _unitOfWork.GenericRepository<OrderHeader>().GetTAsync(predicate: o => o.Id == orderId, includeProperties: includes);
            }
            // For specific user panel uses
            return await _unitOfWork.GenericRepository<OrderHeader>().GetTAsync(predicate: o => o.ApplicationUserId == userId && o.Id == orderId, includeProperties: includes);
        }

        public async Task SetOrderTrackingId(string orderId)
        {
            var orderToSetTrackingId = await this.GetOrder(orderId, null);
            if (orderToSetTrackingId != null && orderToSetTrackingId.TrackingNumber == null)
            {
                orderToSetTrackingId.TrackingNumber = TrackingIdGenerator.GenerateTrackingId();
                orderToSetTrackingId.OrderStatus = OrdersStatus.Preparing.ToString();
                await _unitOfWork.SaveAsync();
            }
            return; // If tracking number already exists, do nothing.
        }

        public async Task UpdateOrder(OrderHeader order, string userId)
        {
            var orderToUpdate = await this.GetOrder(order.Id, userId);
            if (orderToUpdate == null) throw new ApiException(System.Net.HttpStatusCode.NotFound, message: "The requested order could not be found.");
            if (order.OrderStatus != null) orderToUpdate.OrderStatus = order.OrderStatus;
            if (order.PaymentStatus != null) orderToUpdate.PaymentStatus = order.PaymentStatus;
            if (order.PaymentType != null) orderToUpdate.PaymentType = order.PaymentType;
            if (orderToUpdate.OrderStatus == OrdersStatus.Accepted.ToString()) orderToUpdate.TrackingNumber = TrackingNumberGenerator.GenerateTrackingNumber();
            await _unitOfWork.SaveAsync();
        }
    }
}