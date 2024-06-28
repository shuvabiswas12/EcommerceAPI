using ShoppingBasketAPI.Data.UnitOfWork;
using ShoppingBasketAPI.Domain;
using ShoppingBasketAPI.DTOs;
using ShoppingBasketAPI.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasketAPI.Services.Services
{
    public class ShoppingCartServices : IShoppingCartServices
    {
        private IUnitOfWork _unitOfWork;

        public ShoppingCartServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task AddProductToShoppingCart(ShoppingCartCreateRequestDTO shoppingCartCreateRequestDTO)
        {
            throw new NotImplementedException();
        }

        public async Task<ShoppingCartResponseDTO> GetShoppingCartsByUserId(string userId)
        {
            var carts = await _unitOfWork.GenericRepository<ShoppingCart>().GetAllAsync(includeProperties: "Product", c => c.ApplicationUserId == userId);
            ShoppingCartResponseDTO shoppingCartResponse = new ShoppingCartResponseDTO { ShoppingCarts = carts };
            if (carts != null)
            {
                shoppingCartResponse.TotalCost = carts
                    .Where(cart => cart.Product != null)
                    .Sum(cart => (double)cart.Product!.Price);
            }
            return shoppingCartResponse;
        }

        public Task RemoveProductsFromShoppingCart(List<string> productId)
        {
            throw new NotImplementedException();
        }
    }
}
