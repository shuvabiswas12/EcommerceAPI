using Microsoft.AspNetCore.Mvc;
using EcommerceAPI.Domain;
using EcommerceAPI.DTOs;
using EcommerceAPI.DTOs.GenericResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAPI.Services.IServices
{
    public interface IOrderServices
    {
        public Task<GenericResponseDTO<OrderDTO>> GetAllOrders(string? userId = null);
        public Task<OrderDTO> GetOrder(string orderId, string userId);
        public Task<OrderHeader> CreateOrder(ShippingAddressDTO shippingAddressDTO, string userId, string paymentIntentId);
        public Task UpdateOrder(OrderHeader order, string userId);
        public Task CancelOrder(string orderId, string userId);
    }
}
