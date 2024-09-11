using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EcommerceAPI.DTOs;
using EcommerceAPI.Services.IServices;
using EcommerceAPI.Utilities;
using EcommerceAPI.Utilities.Filters;
using EcommerceAPI.Utilities.Validation;

namespace EcommerceAPI.Api.Controllers
{
    /// <summary>
    /// Controller for managing featured products in the Shopping Basket API.
    /// </summary>
    [Route("api/admin/[controller]")]
    [ApiController]
    public class FeaturedProductsController : ControllerBase
    {
        private IFeaturedProductServices _featuredProductServices;

        /// <summary>
        /// Constructor for FeaturedProductsController.
        /// </summary>
        public FeaturedProductsController(IFeaturedProductServices featuredProductServices)
        {
            _featuredProductServices = featuredProductServices;
        }

        /// <summary>
        /// Sets a product as featured based on its ID.
        /// </summary>
        /// <param name="id">The ID of the product to set as featured.</param>
        [HttpPost("{id}"), ApiKeyRequired, Authorize(Roles = "Admin")]
        public async Task<IActionResult> SetProductAsFeatured([FromRoute] string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(new { Error = "Product id must be given." });
            }

            await _featuredProductServices.AddProductAsFeatured(new FeaturedProductRequestDTO { Id = id });
            return StatusCode(StatusCodes.Status201Created, new { Message = "Product added as featured." });
        }

        /// <summary>
        /// Removes a product from the list of featured products based on its ID.
        /// </summary>
        /// <param name="id">The ID of the product to remove from featured.</param>
        [HttpDelete("{id}"), ApiKeyRequired, Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveProductFromFeatured([FromRoute] string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(new { Error = "Product id must be given." });
            }

            await _featuredProductServices.RemoveProductFromFeatured(new FeaturedProductRequestDTO { Id = id });
            return StatusCode(StatusCodes.Status200OK, new { Message = "Product successfully removed from featured." });
        }
    }
}