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

        public async Task<OrderHeader> CreateOrder(ShippingAddressDTO shippingAddressDTO, string userId, string paymentIntentId)
        {
            if (shippingAddressDTO == null) throw new ArgumentNullException(nameof(shippingAddressDTO));

            if (string.IsNullOrEmpty(userId)) throw new ArgumentNullException(nameof(userId));

            // Validate user existence
            var user = await _authenticationServices.GetUserByIdAsync(userId);
            if (user == null) throw new Exception("User not found.");

            var paymentIntentService = new PaymentIntentService();
            var paymentIntent = await paymentIntentService.GetAsync(paymentIntentId);

            if (paymentIntent.Status != "succeeded")
            {
                throw new Exception("Payment was not successful.");
            }

            // Create a new OrderHeader object
            var orderHeader = new OrderHeader
            {
                FullName = shippingAddressDTO.FullName,
                Phone = shippingAddressDTO.Phone,
                Email = shippingAddressDTO.Email,
                HouseName = shippingAddressDTO.HouseName,
                RoadNumber = shippingAddressDTO.RoadNumber,
                City = shippingAddressDTO.City,
                Country = shippingAddressDTO.Country,
                State = shippingAddressDTO.State,
                PostCode = shippingAddressDTO.PostCode,
                OrderStatus = Status.OrderStatus_Pending,
                ApplicationUserId = user.Id,
                OrderAmount = paymentIntent.Amount,
                Currency = paymentIntent.Currency,
                Province = shippingAddressDTO.Province,
                AddressLine1 = shippingAddressDTO.AddressLine1,
                AddressLine2 = shippingAddressDTO.AddressLine2,
                AlternatePhone = shippingAddressDTO.AlternatePhone,
                LandMark = shippingAddressDTO.LandMark,
                PaymentStatus = Status.PaymentStatus_Paid,
                PaymentType = Status.PaymentType_OnlinePayment,
                PaymentIntentId = paymentIntent.Id,
            };

            await _unitOfWork.GenericRepository<OrderHeader>().AddAsync(orderHeader);
            await _unitOfWork.SaveAsync();
            return orderHeader;
        }

        public async Task CancelOrder(string orderId, string userId)
        {
            var orderToCancel = await this.GetOrder(orderId, userId);
            if (orderToCancel == null)
            {
                return;
            }
            orderToCancel.OrderStatus = Status.OrderStatus_Canceled;
            await _unitOfWork.SaveAsync();
        }

        public async Task<GenericResponseDTO<OrderHeader>> GetAllOrders(string userId)
        {
            var orders = await _unitOfWork.GenericRepository<OrderHeader>().GetAllAsync(predicate: o => o.ApplicationUserId == userId, includeProperties: "OrderDetails");
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
            if (orderToUpdate == null)
            {
                throw new NotFoundException(message: "Order is not available."); ;
            }
            if (order.OrderStatus != null) orderToUpdate.OrderStatus = order.OrderStatus;
            if (order.PaymentStatus != null) orderToUpdate.PaymentStatus = order.PaymentStatus;
            if (order.PaymentType != null) orderToUpdate.PaymentType = order.PaymentType;
            if (orderToUpdate.OrderStatus == Status.OrderStatus_Accepted) orderToUpdate.TrackingNumber = TrackingNumberGenerator.GenerateTrackingNumber();

            await _unitOfWork.SaveAsync();
        }
    }
}
