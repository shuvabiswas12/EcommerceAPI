using Asp.Versioning;
using EcommerceAPI.DTOs;
using EcommerceAPI.Services.IServices;
using EcommerceAPI.Utilities.ApplicationRoles;
using EcommerceAPI.Utilities.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Api.Controllers
{
    /// <summary>
    /// Controller for managing featured products in the Shopping Basket API.
    /// </summary>
    [ApiVersion(2.0)]
    [Route("api/admin/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiKeyRequired, Authorize(Roles = ApplicationRoles.ADMIN)]
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
        [HttpPost("{id}")]
        public async Task<IActionResult> SetProductAsFeatured([FromRoute] string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(new { Error = "Product id must be given." });
            }

            await _featuredProductServices.AddProductAsFeatured(new FeaturedProductRequestDTO { Id = id });
            return StatusCode(StatusCodes.Status201Created);
        }

        /// <summary>
        /// Removes a product from the list of featured products based on its ID.
        /// </summary>
        /// <param name="id">The ID of the product to remove from featured.</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveProductFromFeatured([FromRoute] string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(new { Error = "Product id must be given." });
            }

            await _featuredProductServices.RemoveProductFromFeatured(new FeaturedProductRequestDTO { Id = id });
            return NoContent();
        }
    }
}