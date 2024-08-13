using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingBasketAPI.DTOs;
using ShoppingBasketAPI.Services.IServices;
using ShoppingBasketAPI.Utilities.ApplicationRoles;
using ShoppingBasketAPI.Utilities.Exceptions;
using ShoppingBasketAPI.Utilities.Exceptions.Handler;
using ShoppingBasketAPI.Utilities.Filters;

namespace ShoppingBasketAPI.Api.Controllers
{
    /// <summary>
    /// Controller for managing product quantities.
    /// </summary>
    [ApiController]
    [Route("api/admin/[controller]")]
    public class QuantityController : ControllerBase
    {
        private readonly IQuantityServices _quantityServices;
        private readonly ExceptionHandler<QuantityController> _exceptionHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuantityController"/> class.
        /// </summary>
        /// <param name="quantityServices">The quantity services to handle product quantities.</param>
        /// <param name="exceptionHandler">The exception handler for logging and managing exceptions.</param>
        public QuantityController(IQuantityServices quantityServices, ExceptionHandler<QuantityController> exceptionHandler)
        {
            _quantityServices = quantityServices;
            _exceptionHandler = exceptionHandler;
        }

        /// <summary>
        /// Adds the specified quantity to the product's availability.
        /// </summary>
        /// <param name="productId">The ID of the product.</param>
        /// <param name="productQuantity">The product quantity data transfer object containing the quantity to add.</param>
        /// <returns>A task that represents the asynchronous operation, containing the result of the operation.</returns>
        [HttpPost("{productId}")]
        [Authorize(Roles = ApplicationRoles.ADMIN)]
        [ApiKeyRequired]
        public async Task<IActionResult> AddProductQuantity([FromRoute] string productId, [FromBody] ProductQuantityDTO productQuantity)
        {
            try
            {
                await _quantityServices.AddQuantityAsync(productQuantity.Quantity, productId);
                return Ok(new { Message = "Product quantity added successfully." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                _exceptionHandler.HandleException(ex, ex.Message);
                return StatusCode(500, new { Message = "An error occurred while adding product quantity.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Reduces the specified quantity from the product's availability.
        /// </summary>
        /// <param name="productId">The ID of the product.</param>
        /// <param name="productQuantity">The product quantity data transfer object containing the quantity to reduce.</param>
        /// <returns>A task that represents the asynchronous operation, containing the result of the operation.</returns>
        [HttpPut("{productId}")]
        [ApiKeyRequired]
        [Authorize(Roles = ApplicationRoles.ADMIN)]
        public async Task<IActionResult> RemoveProductQuantity([FromRoute] string productId, [FromBody] ProductQuantityDTO productQuantity)
        {
            try
            {
                await _quantityServices.ReduceQuantityAsync(productId, productQuantity.Quantity);
                return Ok(new { Message = "Product quantity reduced successfully." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _exceptionHandler.HandleException(ex, ex.Message);
                return StatusCode(500, new { Message = "An error occurred while reducing product quantity.", Error = ex.Message });
            }
        }
    }
}
