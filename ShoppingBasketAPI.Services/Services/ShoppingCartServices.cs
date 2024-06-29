using ShoppingBasketAPI.Data.UnitOfWork;
using ShoppingBasketAPI.Domain;
using ShoppingBasketAPI.DTOs;
using ShoppingBasketAPI.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

        public async Task AddProductToShoppingCart(ShoppingCartCreateRequestDTO shoppingCartCreateRequestDTO)
        {
            // Check if the same product is found for same user
            var existingShoppingCart = await _unitOfWork.GenericRepository<ShoppingCart>().GetTAsync(predicate: c =>
            c.ApplicationUserId == shoppingCartCreateRequestDTO.UserId && c.ProductId == shoppingCartCreateRequestDTO.ProductId);

            // If cart is not found, create one new cart.
            if (existingShoppingCart == null)
            {
                var newlyCreatedShoppingCart = await _unitOfWork.GenericRepository<ShoppingCart>().AddAsync(new ShoppingCart
                {
                    ApplicationUserId = shoppingCartCreateRequestDTO.UserId,
                    Count = shoppingCartCreateRequestDTO.Count,
                    ProductId = shoppingCartCreateRequestDTO.ProductId,
                });
                await _unitOfWork.SaveAsync();
                await Task.CompletedTask;
                return;
            }

            // If count is greater than zeros. Update the existing one,
            if (shoppingCartCreateRequestDTO.Count > 0)
            {
                existingShoppingCart.Count = shoppingCartCreateRequestDTO.Count;
                await _unitOfWork.SaveAsync();
                await Task.CompletedTask;
                return;
            }

            await Task.CompletedTask;
            return;
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

        public async Task RemoveProductsFromShoppingCart(List<string> productIDs, string userId)
        {
            if (productIDs == null || productIDs.Count == 0)
            {
                throw new ArgumentException("No product IDs provided.", nameof(productIDs));
            }

            if (string.IsNullOrEmpty(userId))
            {
                throw new Exception("No user IDs provided.");
            }

            if (productIDs.Count == 1)
            {
                string productId = productIDs[0];
                var productToDeleteFromCart = await _unitOfWork.GenericRepository<ShoppingCart>()
                    .GetTAsync(x => x.ProductId == productId && x.ApplicationUserId == userId)
                    ?? throw new Exception(message: "The product you select to remove is already removed from Cart.");

                await _unitOfWork.GenericRepository<ShoppingCart>().DeleteAsync(productToDeleteFromCart);
                await _unitOfWork.SaveAsync();
                await Task.CompletedTask;
                return;
            }
            else
            {
                var productsToDeleteFromCart = await _unitOfWork.GenericRepository<ShoppingCart>()
                                                        .GetAllAsync(predicate: x => productIDs.Contains(x.ProductId) && x.ApplicationUserId == userId);

                if (productsToDeleteFromCart == null || !productsToDeleteFromCart.Any())
                {
                    throw new Exception("None of the selected products were found in the cart.");
                }
                await _unitOfWork.GenericRepository<ShoppingCart>().DeleteRangeAsync(productsToDeleteFromCart);
                await Task.CompletedTask;
            }
        }
    }
}
