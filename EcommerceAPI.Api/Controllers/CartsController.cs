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
        public async Task<IActionResult> GetCartsById()
        {
            var userId = (User.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.Actor)?.Value;
            var response = await _shoppingCartServices.GetShoppingCartsByUserId(userId ?? throw new Exception("User id is empty."));
            return Ok(response);
        }

        /// <summary>
        /// Adds a product to the shopping cart or updates the product quantity in the cart.
        /// </summary>
        [HttpPost, ApiKeyRequired, Authorize(Roles = "Web_User")]
        public async Task<IActionResult> AddProductIntoCart([FromBody] CartCreateDTO shoppingCartCreateRequestDTO)
        {
            var userid = (User.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.Actor)?.Value;
            shoppingCartCreateRequestDTO.UserId = userid ?? throw new Exception(message: "User id is not found.");

            // If count is greater than zeros, cart will be added or updated.
            if (shoppingCartCreateRequestDTO.Count > 0)
            {
                await _shoppingCartServices.AddProductToShoppingCart(shoppingCartCreateRequestDTO);
                return StatusCode(StatusCodes.Status201Created, new { Message = "Added" });
            }

            // If count is zero or less than zeros, cart will be deleted.
            // Redirect to DeleteProductsFromCart Action method.
            else
            {
                var productIds = new List<string> { shoppingCartCreateRequestDTO.ProductId };
                return await DeleteProductsFromCart(productIds);  // Invoked the another action method here.
            }
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
            var userId = (User.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.Actor)?.Value
                    ?? throw new Exception(message: "User id is not found.");

            await _shoppingCartServices.RemoveProductsFromShoppingCart(products, userId);
            return StatusCode(StatusCodes.Status204NoContent, new { Message = ResponseMessages.StatusCode_200_DeleteMessage });
        }
    }
}