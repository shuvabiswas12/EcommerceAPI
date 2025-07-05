using Asp.Versioning;
using AutoMapper;
using EcommerceAPI.Domain;
using EcommerceAPI.DTOs;
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
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="WishlistController"/> class.
        /// </summary>
        public WishlistController(IWishlistServices wishlistServices, IMapper mapper)
        {
            _wishlistServices = wishlistServices;
            _mapper = mapper;
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
            var products = wishlists.Select(w => w.Product);
            return Ok(new WishlistDTO
            {
                User = userId,
                Products = _mapper.Map<IEnumerable<ProductDTO>>(products)
            });
        }

        /// <summary>
        /// 'POST' request for inserting product into wishlist.
        /// 'DELETE' request for deleting product from wishlist.
        /// </summary>
        [HttpPost("{productId}"), HttpDelete("{productId}")]
        public async Task<IActionResult> CreateOrDeleteWishlist(string productId)
        {
            var userId = User.GetUserId();

            var existedWishlist = await _wishlistServices.GetWishList(productId, userId);

            // Delete operation
            if (HttpContext.Request.Method == HttpMethods.Delete)
            {
                if (existedWishlist == null)
                {
                    return NoContent();
                }

                await _wishlistServices.RemoveProductFromWishlist(productId, userId!);
                return NoContent();
            }

            // Create operation
            if (HttpContext.Request.Method == HttpMethods.Post)
            {
                if (existedWishlist != null)
                {
                    return StatusCode(StatusCodes.Status201Created);
                }

                await _wishlistServices.AddProductToWishlist(productId, userId);
                return StatusCode(StatusCodes.Status201Created);
            }
            return BadRequest("Invalid HTTP method");
        }
    }
}