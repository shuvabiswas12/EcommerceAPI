using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingBasketAPI.DTOs;
using ShoppingBasketAPI.Services.IServices;
using ShoppingBasketAPI.Utilities;
using ShoppingBasketAPI.Utilities.Exceptions;
using ShoppingBasketAPI.Utilities.Validation;

namespace ShoppingBasketAPI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeaturedProductsController : ControllerBase
    {
        private IFeaturedProductServices _featuredProductServices;
        private ILogger<FeaturedProductsController> _logger;

        public FeaturedProductsController(IFeaturedProductServices featuredProductServices, ILogger<FeaturedProductsController> logger)
        {
            _featuredProductServices = featuredProductServices;
            _logger = logger;
        }

        [HttpPost("{id}")]
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
                _logger.LogError(ex, "\nAn error occured while adding product as featured.\n" + ex.Message + "\n");
                return StatusCode(500, new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "\nAn error occured while adding product as featured.\n");
                return StatusCode(500, new { Error = ResponseMessages.StatusCode_500_ErrorMessage });
            }

        }

        [HttpDelete("{id}")]
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
                _logger.LogError(ex, "\nAn error occured while removing product from featured.\n" + ex.Message + "\n");
                return StatusCode(500, new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "\nAn error occured while removing product from featured.\n");
                return StatusCode(500, new { Error = ResponseMessages.StatusCode_500_ErrorMessage });
            }
        }
    }
}
