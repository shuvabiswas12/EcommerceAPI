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
using AutoMapper;

namespace EcommerceAPI.Api.Controllers
{
    /// <summary>
    /// Controller to manage shopping carts.
    /// </summary>
    [Route("api/v{version:apiVersion}/[controller]"), ApiVersion(1.0)]
    [ApiController, ApiKeyRequired, Authorize(Roles = $"{ApplicationRoles.WEB_USER}")]
    public class CartsController : ControllerBase
    {
        private readonly IShoppingCartServices _shoppingCartServices;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="CartsController"/> class.
        /// </summary>
        public CartsController(IShoppingCartServices shoppingCartServices, IMapper mapper)
        {
            _shoppingCartServices = shoppingCartServices;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves a shopping cart by its ID.
        /// </summary>
        [HttpGet("")]
        public async Task<IActionResult> GetCartByUserId()
        {
            var userId = User.GetUserId();
            var response = await _shoppingCartServices.GetShoppingCartsByUserId(userId);
            return Ok(new CartResponseDTO
            {
                Carts = _mapper.Map<IEnumerable<CartDTO>>(response),
                TotalCost = response.Where(cart => cart.Product != null)
                                    .Sum(cart => cart.Product!.Price * cart.Count)
            });
        }

        /// <summary>
        /// Adds a product to the shopping cart or updates the product quantity in the cart.
        /// </summary>
        [HttpPost]
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
        [HttpDelete]
        public async Task<IActionResult> DeleteProductsFromCart([FromBody] List<string> productsId)
        {
            if (!productsId.Any())
            {
                return BadRequest(new { Error = "No product IDs provided." });
            }
            var userId = User.GetUserId();

            await _shoppingCartServices.RemoveProductsFromShoppingCart(productsId, userId);
            return NoContent();
        }
    }
}