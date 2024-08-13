using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingBasketAPI.DTOs;
using ShoppingBasketAPI.Services.IServices;
using ShoppingBasketAPI.Utilities;
using ShoppingBasketAPI.Utilities.Exceptions.Handler;
using ShoppingBasketAPI.Utilities.Filters;
using ShoppingBasketAPI.Utilities.Validation;

namespace ShoppingBasketAPI.Api.Controllers
{
    /// <summary>
    /// Controller for managing discounts related to products in the Shopping Basket API.
    /// </summary>
    [Route("api/admin/[controller]")]
    [ApiController]
    public class DiscountsController : ControllerBase
    {
        private readonly IDiscountServices _discountServices;
        private readonly ExceptionHandler<DiscountsController> _exceptionHandler;

        /// <summary>
        /// Constructor for DiscountsController.
        /// </summary>
        /// <param name="discountServices">The service handling discount operations.</param>
        /// <param name="exceptionHandler">Exception handler for handling controller-level exceptions.</param>
        public DiscountsController(IDiscountServices discountServices, ExceptionHandler<DiscountsController> exceptionHandler)
        {
            _discountServices = discountServices;
            _exceptionHandler = exceptionHandler;
        }

        /// <summary>
        /// Sets a discount for a product identified by its ID.
        /// </summary>
        /// <param name="id">The ID of the product for which to set the discount.</param>
        /// <param name="discountRequestDTO">DTO containing discount details.</param>
        /// <returns>Returns a status code indicating the result of the operation.</returns>
        [HttpPost("{id}"), ApiKeyRequired, Authorize(Roles = "Admin")]
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
                return _exceptionHandler.HandleException(ex, "An error occured while adding the product discount.");
            }
        }

        /// <summary>
        /// Removes the discount for a product identified by its ID.
        /// </summary>
        /// <param name="id">The ID of the product for which to remove the discount.</param>
        /// <returns>Returns a status code indicating the result of the operation.</returns>
        [HttpDelete("{id}"), ApiKeyRequired, Authorize(Roles = "Admin")]
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
                return _exceptionHandler.HandleException(ex, "An error occured while removing the product discount.");
            }
        }
    }
}
