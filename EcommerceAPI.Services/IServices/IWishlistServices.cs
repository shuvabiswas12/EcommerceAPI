using EcommerceAPI.Domain;
using EcommerceAPI.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAPI.Services.IServices
{
    public interface IWishlistServices
    {
        public Task AddProductToWishlist(string productId, string userId);

        public Task RemoveProductFromWishlist(string productId, string userId);

        public Task<IEnumerable<Wishlist>> GetAllProductsFromWishlists(string userId);

        public Task<Wishlist> GetWishList(string productId, string userId);
    }
}
