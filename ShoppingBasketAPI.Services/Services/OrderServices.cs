using Microsoft.AspNetCore.Mvc;
using ShoppingBasketAPI.Data.UnitOfWork;
using ShoppingBasketAPI.Domain;
using ShoppingBasketAPI.DTOs.GenericResponse;
using ShoppingBasketAPI.Services.IServices;
using ShoppingBasketAPI.Utilities;
using ShoppingBasketAPI.Utilities.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasketAPI.Services.Services
{
    public class OrderServices : IOrderServices
    {
        private IUnitOfWork _unitOfWork;

        public OrderServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<OrderHeader> CreateOrder()
        {
            throw new NotImplementedException();
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
