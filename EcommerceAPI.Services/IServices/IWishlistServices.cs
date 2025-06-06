﻿using EcommerceAPI.Domain;
using EcommerceAPI.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAPI.Services.IServices
{
    /// <summary>
    /// Interface for Wishlist Services.
    /// </summary>
    public interface IWishlistServices
    {
        /// <summary>
        /// Adds a product to the wishlist.
        /// </summary>
        /// <param name="productId">The ID of the product to add to the wishlist.</param>
        /// <param name="userId">The ID of the user whose wishlist is being modified.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task AddProductToWishlist(string productId, string userId);

        /// <summary>
        /// Removes a product from the wishlist.
        /// </summary>
        /// <param name="productId">The ID of the product to remove from the wishlist.</param>
        /// <param name="userId">The ID of the user whose wishlist is being modified.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task RemoveProductFromWishlist(string productId, string userId);

        /// <summary>
        /// Retrieves all products from the wishlists of a specified user.
        /// </summary>
        /// <param name="userId">The ID of the user whose wishlists are being retrieved.</param>
        /// <returns>A task representing the asynchronous operation, containing a collection of wishlists.</returns>
        public Task<WishlistDTO> GetAllProductsFromWishlists(string userId);

        public Task<Wishlist> GetWishList(string productId, string userId);
    }
}
