using ShoppingBasketAPI.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasketAPI.Services.IServices
{
    public interface IDiscountServices
    {
        public Task AddDiscount(string id, DiscountRequestDTO discountRequestDTO);
        public Task RemoveDiscount(string id);
    }
}
