using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EcommerceAPI.DTOs;
using EcommerceAPI.Services.IServices;
using EcommerceAPI.Utilities.ApplicationRoles;
using EcommerceAPI.Utilities.Filters;
using EcommerceAPI.Utilities.Exceptions;

namespace EcommerceAPI.Api.Controllers
{
    /// <summary>
    /// Controller for managing product quantities.
    /// </summary>
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(Roles = ApplicationRoles.ADMIN), ApiKeyRequired]
    public class QuantityController : ControllerBase
    {
        private readonly IQuantityServices _quantityServices;

        /// <summary>
        /// Constructor and DI services initialization
        /// </summary>
        public QuantityController(IQuantityServices quantityServices)
        {
            _quantityServices = quantityServices;
        }

        /// <summary>
        /// Add or Update the specified quantity to the product's availability.
        /// </summary>
        [HttpPost("{productId}")]
        public async Task<IActionResult> ManageProductQuantity([FromRoute] string productId, [FromBody] ProductQuantityDTO payload)
        {
            if (string.IsNullOrEmpty(productId))
            {
                throw new ArgumentNullException(nameof(productId), "Route value 'Product-Id' must be given.");
            }

            if (payload.Quantity < 0)
            {
                throw new ModelValidationException(nameof(payload.Quantity), new string[] { "Quantity should be zero or any positive values." });
            }

            await _quantityServices.ModifyQuantityAsync(payload.Quantity, productId);
            return StatusCode(StatusCodes.Status201Created);
        }
    }
}