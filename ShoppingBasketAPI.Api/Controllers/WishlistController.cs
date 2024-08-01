using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingBasketAPI.Services.IServices;
using ShoppingBasketAPI.Utilities.ApplicationRoles;
using ShoppingBasketAPI.Utilities.Exceptions.Handler;
using ShoppingBasketAPI.Utilities.Filters;
using System.Security.Claims;

namespace ShoppingBasketAPI.Api.Controllers
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
        private readonly ExceptionHandler<WishlistController> _exceptionHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="WishlistController"/> class.
        /// </summary>
        /// <param name="wishlistServices">The wishlist services.</param>
        /// <param name="exceptionHandler">The exception handler for this controller.</param>
        public WishlistController(IWishlistServices wishlistServices, ExceptionHandler<WishlistController> exceptionHandler)
        {
            _wishlistServices = wishlistServices;
            _exceptionHandler = exceptionHandler;
        }

        /// <summary>
        /// Gets all products in the user's wishlist.
        /// </summary>
        /// <returns>A list of products in the user's wishlist.</returns>
        /// <response code="200">Returns the list of products in the wishlist.</response>
        /// <response code="400">If there is an error with the request parameters.</response>
        /// <response code="500">If there is a server-side error while processing the request.</response>
        [HttpGet("")]
        public async Task<IActionResult> GetWishlist()
        {
            try
            {
                var userId = (User.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.Actor)?.Value;
                var wishlists = await _wishlistServices.GetAllProductsFromWishlists(userId);
                return Ok(wishlists);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return _exceptionHandler.HandleException(ex, "There is an error while getting your wishlist.");
            }
        }

        /// <summary>
        /// Creates or deletes a product in the user's wishlist based on the HTTP method.
        /// </summary>
        /// <param name="productId">The ID of the product to be added or removed from the wishlist.</param>
        /// <returns>A status message indicating the result of the operation.</returns>
        /// <response code="200">If the operation was successful.</response>
        /// <response code="302">If the wishlist item already exists (on POST request).</response>
        /// <response code="400">If the request is invalid.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="404">If the wishlist item to be deleted does not exist.</response>
        /// <response code="500">If there is a server-side error while processing the request.</response>
        [HttpPost("{productId}")]
        public async Task<IActionResult> CreateOrDeleteWishlist(string productId)
        {
            try
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
            catch (Exception ex)
            {
                return _exceptionHandler.HandleException(ex, "There is an error while processing your request.");
            }
        }
    }
}
