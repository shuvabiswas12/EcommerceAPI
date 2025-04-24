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

namespace EcommerceAPI.Services.Services
{
    public class OrderServices : IOrderServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthenticationServices _authenticationServices;

        public OrderServices(IUnitOfWork unitOfWork, IAuthenticationServices authenticationServices)
        {
            _unitOfWork = unitOfWork;
            _authenticationServices = authenticationServices;
        }

        public async Task<OrderHeader> CreateOrder(ShippingAddressDTO payload, string userId, string paymentIntentId)
        {
            if (payload == null) throw new ArgumentNullException("You must provide valid shipping address data.");

            if (string.IsNullOrEmpty(userId)) throw new UnauthorizedAccessException("User not authenticated.");

            // Validate user existence
            var user = await _authenticationServices.GetUserByIdAsync(userId);
            if (user == null) throw new UnauthorizedAccessException("User not authenticated.");

            var paymentIntentService = new PaymentIntentService();
            var paymentIntent = await paymentIntentService.GetAsync(paymentIntentId);

            if (paymentIntent.Status != "succeeded") throw new InvalidOperationException("Payment was not successful.");

            // Create a new OrderHeader object
            var orderHeader = new OrderHeader
            {
                OrderStatus = Status.OrderStatus_Pending,
                ApplicationUserId = user.Id,
                OrderAmount = paymentIntent.Amount,
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

            await _unitOfWork.GenericRepository<OrderHeader>().AddAsync(orderHeader);
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

        public async Task<GenericResponseDTO<OrderHeader>> GetAllOrders(string? userId = null)
        {
            IEnumerable<OrderHeader> orders = new List<OrderHeader>();

            if (userId is not null)
            {
                // For specific user panel uses
                orders = await _unitOfWork.GenericRepository<OrderHeader>().GetAllAsync(predicate: o => o.ApplicationUserId == userId, includeProperties: "OrderDetails");
            }
            else
            {
                // For admin panel uses
                orders = await _unitOfWork.GenericRepository<OrderHeader>().GetAllAsync(includeProperties: "OrderDetails");
            }
            return new GenericResponseDTO<OrderHeader>
            {
                Count = orders.Count(),
                Data = orders
            };
        }

        public async Task<OrderHeader> GetOrder(string orderId, string userId)
        {
            return await _unitOfWork.GenericRepository<OrderHeader>().GetTAsync(predicate: o => o.ApplicationUserId == userId && o.Id == orderId, includeProperties: "OrderDetails");
        }

        public async Task UpdateOrder(OrderHeader order, string userId)
        {
            var orderToUpdate = await this.GetOrder(order.Id, userId);
            if (orderToUpdate == null) throw new NotFoundException(message: "Order is not available.");
            if (order.OrderStatus != null) orderToUpdate.OrderStatus = order.OrderStatus;
            if (order.PaymentStatus != null) orderToUpdate.PaymentStatus = order.PaymentStatus;
            if (order.PaymentType != null) orderToUpdate.PaymentType = order.PaymentType;
            if (orderToUpdate.OrderStatus == Status.OrderStatus_Accepted) orderToUpdate.TrackingNumber = TrackingNumberGenerator.GenerateTrackingNumber();
            await _unitOfWork.SaveAsync();
        }
    }
}