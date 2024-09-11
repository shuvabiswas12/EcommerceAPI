using EcommerceAPI.Data.UnitOfWork;
using EcommerceAPI.Domain;
using EcommerceAPI.Services.IServices;
using EcommerceAPI.Utilities.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAPI.Services.Services
{
    public class WishlistServices : IWishlistServices
    {
        private IUnitOfWork _unitOfWork;

        public WishlistServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddProductToWishlist(string productId, string userId)
        {
            if (string.IsNullOrEmpty(productId)) throw new ArgumentNullException("Product id should not be nulled.");

            if (string.IsNullOrEmpty(userId)) throw new ArgumentNullException("userId should not be nulled.");

            var existingProduct = await _unitOfWork.GenericRepository<Product>().GetTAsync(p => p.Id == productId);

            if (existingProduct == null) throw new NotFoundException("Product not found.");

            var newWishlist = new Wishlist { ApplicationUserId = userId, ProductId = productId };

            await _unitOfWork.GenericRepository<Wishlist>().AddAsync(newWishlist);
            await _unitOfWork.SaveAsync();
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<Wishlist>> GetAllProductsFromWishlists(string userId)
        {
            if (string.IsNullOrEmpty(userId)) throw new ArgumentNullException("userId should not be nulled.");

            return await _unitOfWork.GenericRepository<Wishlist>().GetAllAsync(includeProperties: "Product");
        }

        public async Task RemoveProductFromWishlist(string productId, string userId)
        {
            if (string.IsNullOrEmpty(productId)) throw new ArgumentNullException("Product id should not be nulled.");

            if (string.IsNullOrEmpty(userId)) throw new ArgumentNullException("userId should not be nulled.");

            var existingProduct = await _unitOfWork.GenericRepository<Wishlist>().GetTAsync(p => p.ProductId == productId && p.ApplicationUserId == userId);

            if (existingProduct == null) throw new NotFoundException("The product is not added yet in Wishlist");

            await _unitOfWork.GenericRepository<Wishlist>().DeleteAsync(existingProduct);
            await _unitOfWork.SaveAsync();
            await Task.CompletedTask;
        }
    }
}
