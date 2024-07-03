using Microsoft.AspNetCore.Mvc;
using ShoppingBasketAPI.Domain;
using ShoppingBasketAPI.DTOs.GenericResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasketAPI.Services.IServices
{
    public interface IOrderServices
    {
        public Task<GenericResponseDTO<OrderHeader>> GetAllOrders(string userId);
        public Task<OrderHeader> GetOrder(string orderId, string userId);
        public Task<OrderHeader> CreateOrder();
        public Task UpdateOrder(string orderId);
        public Task DeleteOrder(string orderId);
    }
}
