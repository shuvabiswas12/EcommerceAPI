using EcommerceAPI.Domain;
using EcommerceAPI.DTOs;
using EcommerceAPI.DTOs.GenericResponse;
using EcommerceAPI.Utilities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAPI.Services.IServices
{
    public interface IOrderServices
    {
        public Task<IEnumerable<OrderHeader>> GetAllOrders(string? userId = null, OrdersStatus? orderStatus = null, PaymentStatus? paymentStatus = null, PaymentType? paymentType = null, SortBy? sortBy = SortBy.DateDESC);
        public Task<OrderHeader> GetOrder(string orderId, string? userId);
        public Task<string> CreateOrder(ShippingAddressDTO shippingAddressDTO, string userId, string paymentIntentId);
        public Task UpdateOrder(OrderHeader order, string userId);
        public Task CancelOrder(string orderId, string? userId);
        public Task SetOrderTrackingId(string orderId);
    }
}
