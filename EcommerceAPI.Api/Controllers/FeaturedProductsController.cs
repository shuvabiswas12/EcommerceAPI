using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EcommerceAPI.DTOs;
using EcommerceAPI.Services.IServices;
using EcommerceAPI.Utilities;
using EcommerceAPI.Utilities.Exceptions;
using EcommerceAPI.Utilities.Exceptions.Handler;
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
        private ExceptionHandler<FeaturedProductsController> _exceptionHandler;

        /// <summary>
        /// Constructor for FeaturedProductsController.
        /// </summary>
        /// <param name="featuredProductServices">The service handling featured product operations.</param>
        /// <param name="exceptionHandler">Exception handler for handling controller-level exceptions.</param>
        public FeaturedProductsController(IFeaturedProductServices featuredProductServices, ExceptionHandler<FeaturedProductsController> exceptionHandler)
        {
            _featuredProductServices = featuredProductServices;
            _exceptionHandler = exceptionHandler;
        }

        /// <summary>
        /// Sets a product as featured based on its ID.
        /// </summary>
        /// <param name="id">The ID of the product to set as featured.</param>
        /// <returns>Returns a status code indicating the result of the operation.</returns>
        [HttpPost("{id}"), ApiKeyRequired, Authorize(Roles = "Admin")]
        public async Task<IActionResult> SetProductAsFeatured([FromRoute] string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(new { Error = "Product id must be given." });
            }

            try
            {
                await _featuredProductServices.AddProductAsFeatured(new FeaturedProductRequestDTO { Id = id });
                return StatusCode(StatusCodes.Status201Created, new { Message = "Product added as featured." });
            }
            catch (DuplicateEntriesFoundException ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return _exceptionHandler.HandleException(ex, "An error occured while adding product as featured.");
            }

        }

        /// <summary>
        /// Removes a product from the list of featured products based on its ID.
        /// </summary>
        /// <param name="id">The ID of the product to remove from featured.</param>
        /// <returns>Returns a status code indicating the result of the operation.</returns>
        [HttpDelete("{id}"), ApiKeyRequired, Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveProductFromFeatured([FromRoute] string id)
        {

            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(new { Error = "Product id must be given." });
            }

            try
            {
                await _featuredProductServices.RemoveProductFromFeatured(new FeaturedProductRequestDTO { Id = id });
                return StatusCode(StatusCodes.Status200OK, new { Message = "Product successfully removed from featured." });
            }
            catch (NotFoundException ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return _exceptionHandler.HandleException(ex, "An error occured while removing product from featured.");
            }
        }
    }
}
