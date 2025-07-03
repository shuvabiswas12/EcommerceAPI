using Microsoft.AspNetCore.Mvc;
using EcommerceAPI.Data.UnitOfWork;
using EcommerceAPI.Domain;
using EcommerceAPI.DTOs;
using EcommerceAPI.DTOs.GenericResponse;
using EcommerceAPI.Services.IServices;
using EcommerceAPI.Utilities;
using EcommerceAPI.Utilities.Exceptions;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using AutoMapper;
using System.Net;

namespace EcommerceAPI.Services.Services
{
    public class OrderServices : IOrderServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthenticationServices _authenticationServices;
        private readonly IShoppingCartServices _shoppingCartServices;
        private readonly IMapper _mapper;

        public OrderServices(IUnitOfWork unitOfWork, IAuthenticationServices authenticationServices, IShoppingCartServices shoppingCartServices, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _authenticationServices = authenticationServices;
            _shoppingCartServices = shoppingCartServices;
            _mapper = mapper;
        }

        public async Task<OrderHeader> CreateOrder(ShippingAddressDTO payload, string userId, string paymentIntentId)
        {
            if (payload == null) throw new ArgumentNullException("You must provide valid shipping address data.");

            if (string.IsNullOrEmpty(userId)) throw new UnauthorizedAccessException("User not authenticated.");

            // Validate user existence
            var user = await _authenticationServices.GetUserByIdAsync(userId);
            if (user == null) throw new UnauthorizedAccessException("User not authenticated.");

            // Get carts and check if there are any products in the shopping cart
            var carts = (await _shoppingCartServices.GetShoppingCartsByUserId(userId)).Carts;
            if (carts?.Count() == 0) throw new ApiException(HttpStatusCode.BadRequest, message: "You cannot create an order without any products in your shopping cart.");

            var paymentIntentService = new PaymentIntentService();
            var paymentIntent = await paymentIntentService.GetAsync(paymentIntentId);

            if (paymentIntent.Status != "succeeded") throw new InvalidOperationException("Payment was not successful.");

            // Create a new OrderHeader object
            var orderHeader = new OrderHeader
            {
                // If user paid with online payment, then order status will be accepted by default. Admin can change it later and refund the money.
                OrderStatus = Status.OrderStatus_Accepted,
                ApplicationUserId = user.Id,
                OrderAmount = paymentIntent.Amount / 100,
                Currency = paymentIntent.Currency,
                PaymentStatus = Status.PaymentStatus_Paid,
                PaymentType = Status.PaymentType_OnlinePayment,
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
                Price = cart.Price,
                OrderHeaderId = orderHeader.Id,
                CreatedAt = orderHeader.OrderDate
            }).ToList();

            await _unitOfWork.GenericRepository<OrderHeader>().AddAsync(orderHeader);

            // Remove products from shopping cart after order creation
            await _shoppingCartServices.RemoveProductsFromShoppingCart(productIDs: carts.Select(c => c.ProductId).ToList(), userId: userId);
            await _unitOfWork.SaveAsync();
            return orderHeader;
        }

        public async Task CancelOrder(string orderId, string userId)
        {
            var orderToCancel = await this.GetOrder(orderId, userId);
            if (orderToCancel == null) return;
            orderToCancel.OrderStatus = Status.OrderStatus_Canceled;
            await _unitOfWork.SaveAsync();
        }

        public async Task<GenericResponseDTO<OrderDTO>> GetAllOrders(string? userId = null)
        {
            IEnumerable<OrderHeader> orders = new List<OrderHeader>();

            if (userId is not null)
            {
                // For specific user panel uses
                orders = await _unitOfWork.GenericRepository<OrderHeader>().GetAllAsync(predicate: o => o.ApplicationUserId == userId, includeProperties: "OrderDetails, OrderAddress, OrderDetails.Product, ApplicationUser", orderBy: o => o.OrderByDescending(x => x.OrderDate));
            }
            else
            {
                // For admin panel uses
                orders = await _unitOfWork.GenericRepository<OrderHeader>().GetAllAsync(includeProperties: "OrderDetails, OrderAddress, OrderDetails.Product, ApplicationUser", orderBy: o => o.OrderByDescending(x => x.OrderDate));
            }
            return new GenericResponseDTO<OrderDTO>
            {
                Count = orders.Count(),
                Data = _mapper.Map<List<OrderDTO>>(orders)
            };
        }

        public async Task<OrderDTO> GetOrder(string orderId, string userId)
        {
            var order = await _unitOfWork.GenericRepository<OrderHeader>().GetTAsync(predicate: o => o.ApplicationUserId == userId && o.Id == orderId, includeProperties: "OrderDetails, OrderAddress, OrderDetails.Product, ApplicationUser");
            return _mapper.Map<OrderDTO>(order);
        }

        public async Task UpdateOrder(OrderHeader order, string userId)
        {
            var orderToUpdate = await this.GetOrder(order.Id, userId);
            if (orderToUpdate == null) throw new ApiException(System.Net.HttpStatusCode.NotFound, message: "The requested order could not be found.");
            if (order.OrderStatus != null) orderToUpdate.OrderStatus = order.OrderStatus;
            if (order.PaymentStatus != null) orderToUpdate.PaymentStatus = order.PaymentStatus;
            if (order.PaymentType != null) orderToUpdate.PaymentType = order.PaymentType;
            if (orderToUpdate.OrderStatus == Status.OrderStatus_Accepted) orderToUpdate.TrackingNumber = TrackingNumberGenerator.GenerateTrackingNumber();
            await _unitOfWork.SaveAsync();
        }
    }
}