using EcommerceAPI.Domain;
using EcommerceAPI.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAPI.Services.IServices
{

    public interface IShoppingCartServices
    {
        public Task<IEnumerable<ShoppingCart>> GetShoppingCartsByUserId(string userId);

        public Task AddProductToShoppingCart(CartCreateDTO shoppingCartCreateRequestDTO);

        public Task RemoveProductsFromShoppingCart(List<string> productIDs, string userId);
    }
}
