using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EcommerceAPI.DTOs;
using EcommerceAPI.Services.IServices;
using EcommerceAPI.Utilities;
using EcommerceAPI.Utilities.Filters;
using System.Security.Claims;

namespace EcommerceAPI.Api.Controllers
{
    /// <summary>
    /// Controller to manage shopping carts.
    /// </summary>
    /// <remarks>
    /// This controller provides actions to manage shopping carts, allowing users to add, remove, and view items in their cart.
    /// </remarks>
    [Route("api/[controller]")]
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
        [HttpGet(""), ApiKeyRequired, Authorize(Roles = "Web_User")]
        public async Task<IActionResult> GetCartByUserId()
        {
            var userId = User.GetUserId();
            var response = await _shoppingCartServices.GetShoppingCartsByUserId(userId);
            return Ok(response);
        }

        /// <summary>
        /// Adds a product to the shopping cart or updates the product quantity in the cart.
        /// </summary>
        [HttpPost, ApiKeyRequired, Authorize(Roles = "Web_User")]
        public async Task<IActionResult> AddProductIntoCart([FromBody] CartCreateDTO shoppingCartCreateRequestDTO)
        {
            var userId = User.GetUserId();

            if (string.IsNullOrEmpty(shoppingCartCreateRequestDTO.ProductId))
            {
                throw new ArgumentNullException(nameof(shoppingCartCreateRequestDTO.ProductId), "Product ID is required.");
            }

            if (shoppingCartCreateRequestDTO.Count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(shoppingCartCreateRequestDTO.Count), "Count cannot be negative.");
            }

            shoppingCartCreateRequestDTO.UserId = userId;

            if (shoppingCartCreateRequestDTO.Count <= 0)
            {
                // If the count is zero or less, delete the product from the cart
                var productIds = new List<string> { shoppingCartCreateRequestDTO.ProductId };
                return await DeleteProductsFromCart(productIds);
            }

            // If the count is greater than zero, add or update the product in the cart
            await _shoppingCartServices.AddProductToShoppingCart(shoppingCartCreateRequestDTO);
            return StatusCode(StatusCodes.Status201Created, new { Message = "Product added or updated in the cart." });
        }

        /// <summary>
        /// Removes products from the shopping cart.
        /// </summary>
        [HttpDelete, ApiKeyRequired, Authorize(Roles = "Web_User")]
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