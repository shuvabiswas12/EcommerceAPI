using Microsoft.AspNetCore.Mvc;
using ShoppingBasketAPI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasketAPI.Services.IServices
{
    public interface IOrderServices
    {
        public Task<IEnumerable<OrderHeader>> GetAllOrders();
        public Task<OrderHeader> GetOrder(string orderId);
        public Task<OrderHeader> CreateOrder();
        public Task UpdateOrder(string orderId);
        public Task DeleteOrder(string orderId);
    }
}
