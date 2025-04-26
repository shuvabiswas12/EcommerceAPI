using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EcommerceAPI.DTOs;
using EcommerceAPI.Services.IServices;
using EcommerceAPI.Utilities;
using EcommerceAPI.Utilities.Filters;
using System.Security.Claims;
using EcommerceAPI.Utilities.Exceptions;
using Asp.Versioning;
using EcommerceAPI.Utilities.ApplicationRoles;

namespace EcommerceAPI.Api.Controllers
{
    /// <summary>
    /// Controller to manage shopping carts.
    /// </summary>
    [Route("api/v{version:apiVersion}/[controller]"), ApiVersion(1.0)]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly IShoppingCartServices _shoppingCartServices;

        /// <summary>
        /// Initializes a new instance of the <see cref="CartsController"/> class.
        /// </summary>
        public CartsController(IShoppingCartServices shoppingCartServices)
        {
            _shoppingCartServices = shoppingCartServices;
        }

        /// <summary>
        /// Retrieves a shopping cart by its ID.
        /// </summary>
        [HttpGet(""), ApiKeyRequired, Authorize(Roles = $"{ApplicationRoles.WEB_USER}")]
        public async Task<IActionResult> GetCartByUserId()
        {
            var userId = User.GetUserId();
            var response = await _shoppingCartServices.GetShoppingCartsByUserId(userId);
            return Ok(response);
        }

        /// <summary>
        /// Adds a product to the shopping cart or updates the product quantity in the cart.
        /// </summary>
        [HttpPost, ApiKeyRequired, Authorize(Roles = $"{ApplicationRoles.WEB_USER}")]
        public async Task<IActionResult> AddProductIntoCart([FromBody] CartCreateDTO payload)
        {
            var userId = User.GetUserId();

            if (string.IsNullOrEmpty(payload.ProductId))
            {
                throw new ModelValidationException(nameof(payload.ProductId), new string[] { "Product ID is required." });
            }

            if (payload.Count < 0)
            {
                throw new ModelValidationException(nameof(payload.Count), new string[] { "Count cannot be negative." });
            }

            payload.UserId = userId;

            if (payload.Count <= 0)
            {
                // If the count is zero or less, delete the product from the cart
                var productIds = new List<string> { payload.ProductId };
                return await DeleteProductsFromCart(productIds);
            }

            // If the count is greater than zero, add or update the product in the cart
            await _shoppingCartServices.AddProductToShoppingCart(payload);
            return StatusCode(StatusCodes.Status201Created);
        }

        /// <summary>
        /// Removes products from the shopping cart.
        /// </summary>
        [HttpDelete, ApiKeyRequired, Authorize(Roles = $"{ApplicationRoles.WEB_USER}")]
        public async Task<IActionResult> DeleteProductsFromCart([FromBody] List<string> products)
        {
            if (!products.Any())
            {
                return BadRequest(new { Error = "No product IDs provided." });
            }
            var userId = User.GetUserId();

            await _shoppingCartServices.RemoveProductsFromShoppingCart(products, userId);
            return NoContent();
        }
    }
}