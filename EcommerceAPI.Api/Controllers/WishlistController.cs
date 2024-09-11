using EcommerceAPI.Services.IServices;
using EcommerceAPI.Utilities.ApplicationRoles;
using EcommerceAPI.Utilities.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EcommerceAPI.Api.Controllers
{
    /// <summary>
    /// Controller for managing wishlist operations.
    /// </summary>
    [Route("api/[controller]")]
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
            var userId = (User.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.Actor)?.Value;
            var wishlists = await _wishlistServices.GetAllProductsFromWishlists(userId);
            return Ok(wishlists);
        }

        /// <summary>
        /// Creates or deletes a product in the user's wishlist based on the HTTP method.
        /// </summary>
        [HttpPost("{productId}")]
        public async Task<IActionResult> CreateOrDeleteWishlist(string productId)
        {
            var userId = (User.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.Actor)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not authenticated.");
            }

            var existedWishlist = await _wishlistServices.GetWishList(userId, productId);

            if (HttpContext.Request.Method == HttpMethods.Delete)
            {
                if (existedWishlist != null)
                {
                    // Delete operation will be executed
                    await _wishlistServices.RemoveProductFromWishlist(productId, userId);
                    return Ok("Wishlist item deleted successfully.");
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, $"Wishlist item with ID {productId} not found.");
                }
            }

            if (HttpContext.Request.Method == HttpMethods.Post)
            {
                if (existedWishlist == null)
                {
                    // Create operation will be executed.
                    await _wishlistServices.AddProductToWishlist(productId, userId);
                    return Ok("Wishlist item created successfully.");
                }
                else
                {
                    return StatusCode(StatusCodes.Status302Found, $"Wishlist item with ID {productId} already exists.");
                }
            }

            return BadRequest("Invalid HTTP method");
        }
    }
}