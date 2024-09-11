using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EcommerceAPI.DTOs;
using EcommerceAPI.Services.IServices;
using EcommerceAPI.Utilities.ApplicationRoles;
using EcommerceAPI.Utilities.Filters;

namespace EcommerceAPI.Api.Controllers
{
    /// <summary>
    /// Controller for managing product quantities.
    /// </summary>
    [ApiController]
    [Route("api/admin/[controller]")]
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
        /// Adds the specified quantity to the product's availability.
        /// </summary>
        [HttpPost("{productId}")]
        [Authorize(Roles = ApplicationRoles.ADMIN)]
        [ApiKeyRequired]
        public async Task<IActionResult> AddProductQuantity([FromRoute] string productId, [FromBody] ProductQuantityDTO productQuantity)
        {
            await _quantityServices.AddQuantityAsync(productQuantity.Quantity, productId);
            return Ok(new { Message = "Product quantity added successfully." });
        }

        /// <summary>
        /// Reduces the specified quantity from the product's availability.
        /// </summary>
        [HttpPut("{productId}")]
        [ApiKeyRequired]
        [Authorize(Roles = ApplicationRoles.ADMIN)]
        public async Task<IActionResult> RemoveProductQuantity([FromRoute] string productId, [FromBody] ProductQuantityDTO productQuantity)
        {
            await _quantityServices.ReduceQuantityAsync(productId, productQuantity.Quantity);
            return Ok(new { Message = "Product quantity reduced successfully." });
        }
    }
}