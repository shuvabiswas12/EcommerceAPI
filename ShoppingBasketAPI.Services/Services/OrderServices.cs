using Microsoft.AspNetCore.Mvc;
using ShoppingBasketAPI.Data.UnitOfWork;
using ShoppingBasketAPI.Domain;
using ShoppingBasketAPI.DTOs.GenericResponse;
using ShoppingBasketAPI.Services.IServices;
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

        public async Task DeleteOrder(string orderId, string userId)
        {
            var orderToDelete = await _unitOfWork.GenericRepository<OrderHeader>().GetTAsync(o => o.ApplicationUserId == userId && o.Id == orderId);
            if (orderToDelete == null)
            {
                return;
            }
            await _unitOfWork.GenericRepository<OrderHeader>().DeleteAsync(orderToDelete);
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

        public Task UpdateOrder(string orderId)
        {
            throw new NotImplementedException();
        }
    }
}
