using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingBasketAPI.DTOs;
using ShoppingBasketAPI.Services.IServices;
using ShoppingBasketAPI.Utilities;
using ShoppingBasketAPI.Utilities.Validation;

namespace ShoppingBasketAPI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountsController : ControllerBase
    {
        private readonly IDiscountServices _discountServices;
        private readonly ILogger<DiscountsController> _logger;

        public DiscountsController(IDiscountServices discountServices, ILogger<DiscountsController> logger)
        {
            _discountServices = discountServices;
            _logger = logger;
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> SetProductDiscount([FromRoute] string id, [FromBody] DiscountRequestDTO discountRequestDTO)
        {
            if (id == null) return BadRequest(new { Error = "Route value id must be given." });

            var modelState = ModelValidator.ValidateModel(discountRequestDTO);
            if (!modelState.IsValid)
            {
                var errors = ModelValidator.GetErrors(modelState);
                return BadRequest(new { Errors = errors });
            }
            try
            {
                await _discountServices.AddDiscount(id, discountRequestDTO);
                return StatusCode(StatusCodes.Status201Created, new { Message = "Successfully added the product discount." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "\nAn error occured while adding the product discount.\n" + ex.Message + "\n");
                return StatusCode(500, new { Error = ResponseMessages.StatusCode_500_ErrorMessage });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveProductDiscount([FromRoute] string id)
        {
            if (id == null) return BadRequest(new { Error = "Route value id must be given." });
            try
            {
                await _discountServices.RemoveDiscount(id);
                return StatusCode(StatusCodes.Status202Accepted, new { Message = "Successfully removed the product discount." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "\nAn error occured while removing the product discount.\n" + ex.Message + "\n");
                return StatusCode(500, new { Error = ResponseMessages.StatusCode_500_ErrorMessage });
            }
        }
    }
}
