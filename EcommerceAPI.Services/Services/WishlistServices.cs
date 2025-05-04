using AutoMapper;
using EcommerceAPI.Data.UnitOfWork;
using EcommerceAPI.Domain;
using EcommerceAPI.DTOs;
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
        private readonly IMapper _mapper;

        public WishlistServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddProductToWishlist(string productId, string userId)
        {
            if (string.IsNullOrEmpty(productId))
            {
                throw new ArgumentNullException("Product id must be provided.");
            }

            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("User not authenticated.");
            }

            var existingProduct = await _unitOfWork.GenericRepository<Product>().GetTAsync(p => p.Id == productId);

            if (existingProduct == null)
            {
                throw new ApiException(System.Net.HttpStatusCode.NotFound, "The Product not found.");
            }
            var newWishlist = new Wishlist { ApplicationUserId = userId, ProductId = productId };

            await _unitOfWork.GenericRepository<Wishlist>().AddAsync(newWishlist);
            await _unitOfWork.SaveAsync();
        }

        public async Task<WishlistDTO> GetAllProductsFromWishlists(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException("User id must be provided.");
            }

            IEnumerable<Wishlist> wishlist = await _unitOfWork.GenericRepository<Wishlist>().GetAllAsync(includeProperties: "Product.Images", predicate: w => w.ApplicationUserId == userId);
            var products = wishlist.Select(w => w.Product);
            return new WishlistDTO { User = userId, Products = _mapper.Map<IEnumerable<ProductDTO>>(products) };
        }

        public async Task<Wishlist> GetWishList(string productId, string userId)
        {
            if (string.IsNullOrEmpty(productId) || string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException("Product ID and User ID must be provided.");
            }

            return await _unitOfWork.GenericRepository<Wishlist>().GetTAsync(
                predicate: w => w.ApplicationUserId == userId && w.ProductId == productId);
        }

        public async Task RemoveProductFromWishlist(string productId, string userId)
        {
            if (string.IsNullOrEmpty(productId))
            {
                throw new ArgumentNullException("Product id must be provided.");
            }

            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException("User id must be provided");
            }

            var existingProduct = await _unitOfWork.GenericRepository<Wishlist>()
                .GetTAsync(p => p.ProductId == productId && p.ApplicationUserId == userId);

            if (existingProduct == null)
            {
                throw new ApiException(System.Net.HttpStatusCode.NotFound, "This product is not in your wishlist");
            }

            await _unitOfWork.GenericRepository<Wishlist>().DeleteAsync(existingProduct);
            await _unitOfWork.SaveAsync();
        }
    }
}