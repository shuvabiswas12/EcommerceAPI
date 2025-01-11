using Asp.Versioning;
using EcommerceAPI.Services.IServices;
using EcommerceAPI.Utilities;
using EcommerceAPI.Utilities.ApplicationRoles;
using EcommerceAPI.Utilities.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Api.Controllers
{
    /// <summary>
    /// Controller for managing wishlist operations.
    /// </summary>
    [Route("api/v{version:apiVersion}/[controller]"), ApiVersion(1.0)]
    [Authorize(Roles = ApplicationRoles.WEB_USER), ApiKeyRequired]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistServices _wishlistServices;

        /// <summary>
        /// Initializes a new instance of the <see cref="WishlistController"/> class.
        /// </summary>
        public WishlistController(IWishlistServices wishlistServices)
        {
            _wishlistServices = wishlistServices;
        }

        /// <summary>
        /// Gets all products in the user's wishlist.
        /// </summary>
        /// <returns>A list of products in the user's wishlist.</returns>
        [HttpGet("")]
        public async Task<IActionResult> GetWishlist()
        {
            var userId = User.GetUserId();
            var wishlists = await _wishlistServices.GetAllProductsFromWishlists(userId);
            return Ok(wishlists);
        }

        /// <summary>
        /// 'POST' request for inserting product into wishlist.
        /// 'DELETE' request for deleting product from wishlist.
        /// </summary>
        [HttpPost("{productId}"), HttpDelete("{productId}")]
        public async Task<IActionResult> CreateOrDeleteWishlist(string productId)
        {
            var userId = User.GetUserId();

            var existedWishlist = await _wishlistServices.GetWishList(userId, productId);

            // Delete operation
            if (HttpContext.Request.Method == HttpMethods.Delete)
            {
                if (existedWishlist == null)
                {
                    return NotFound();
                }

                await _wishlistServices.RemoveProductFromWishlist(productId, userId!);
                return NoContent();
            }

            // Create operation
            if (HttpContext.Request.Method == HttpMethods.Post)
            {
                if (existedWishlist != null)
                {
                    return StatusCode(StatusCodes.Status302Found);
                }

                await _wishlistServices.AddProductToWishlist(productId, userId);
                return StatusCode(StatusCodes.Status201Created);
            }
            return BadRequest("Invalid HTTP method");
        }
    }
}