using Microsoft.AspNetCore.Mvc;
using ShoppingBasketAPI.Data.UnitOfWork;
using ShoppingBasketAPI.Domain;
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

        public Task DeleteOrder(string orderId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<OrderHeader>> GetAllOrders()
        {
            throw new NotImplementedException();
        }

        public Task<OrderHeader> GetOrder(string orderId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateOrder(string orderId)
        {
            throw new NotImplementedException();
        }
    }
}
