using Microsoft.AspNetCore.Mvc;
using ShoppingBasketAPI.Domain;
using ShoppingBasketAPI.DTOs;
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
        public Task<OrderHeader> CreateOrder(ShippingAddressDTO shippingAddressDTO, string userId, string paymentIntentId);
        public Task UpdateOrder(OrderHeader order, string userId);
        public Task CancelOrder(string orderId, string userId);
    }
}
