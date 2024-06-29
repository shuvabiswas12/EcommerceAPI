using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingBasketAPI.DTOs;
using ShoppingBasketAPI.Services.IServices;
using ShoppingBasketAPI.Utilities.Exceptions.Handler;
using ShoppingBasketAPI.Utilities.Filters;
using System.Security.Claims;

namespace ShoppingBasketAPI.Api.Controllers
{
    /// <summary>
    /// Controller to manage shopping carts.
    /// </summary>
    /// <remarks>
    /// This controller provides actions to manage shopping carts, allowing users to add, remove, and view items in their cart.
    /// </remarks>
    /// <response code="200">Success</response>
    /// <response code="400">Bad Request</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden</response>
    /// <response code="404">Not Found</response>
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly IShoppingCartServices _shoppingCartServices;
        private readonly ExceptionHandler<CartsController> _exceptionHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="CartsController"/> class.
        /// </summary>
        /// <param name="shoppingCartServices">The service for managing shopping carts.</param>
        /// <param name="exceptionHandler">The handler for managing exceptions in the <see cref="CartsController"/>.</param>
        public CartsController(IShoppingCartServices shoppingCartServices, ExceptionHandler<CartsController> exceptionHandler)
        {
            _shoppingCartServices = shoppingCartServices;
            _exceptionHandler = exceptionHandler;
        }

        /// <summary>
        /// Retrieves a shopping cart by its ID.
        /// </summary>
        /// <returns>
        /// An <see cref="IActionResult"/> containing the shopping cart details.
        /// </returns>
        /// <response code="200">Success - returns the shopping cart details.</response>
        /// <response code="404">Not Found - the shopping cart was not found.</response>
        /// <response code="401">Unauthorized - API key is missing or invalid.</response>
        [HttpGet("")]
        [ApiKeyRequired, Authorize(Roles = "Web_User")]
        public async Task<IActionResult> GetCartsById()
        {
            var userId = (User.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.Actor)?.Value;
            try
            {
                var response = await _shoppingCartServices.GetShoppingCartsByUserId(userId ?? throw new Exception("User id is empty."));
                return Ok(response);
            }
            catch (Exception ex)
            {
                return _exceptionHandler.HandleException(ex, "Something went wrong when getting Cart details.");
            }
        }


        /// <summary>
        /// Adds a product to the shopping cart or updates the product quantity in the cart.
        /// </summary>
        /// <param name="shoppingCartCreateRequestDTO">
        /// Containing the details of the product 
        /// to be added or updated in the shopping cart.
        /// </param>
        /// <returns>
        /// An indicating the result of the operation.
        /// </returns>
        /// <response code="200">Success - product added or updated in the cart.</response>
        /// <response code="400">Bad Request - invalid product details.</response>
        /// <response code="401">Unauthorized - API key is missing or invalid.</response>
        /// <response code="500">Internal Server Error - an error occurred while processing the request.</response>
        [HttpPost]
        [ApiKeyRequired, Authorize(Roles = "Web_User")]
        public Task<IActionResult> AddProductIntoCart([FromBody] ShoppingCartCreateRequestDTO shoppingCartCreateRequestDTO)
        { throw new NotImplementedException(); }


        /// <summary>
        /// Removes products from the shopping cart.
        /// </summary>
        /// <param name="products">A list of product IDs to be removed from the cart.</param>
        /// <returns>
        /// An indicating the result of the operation.
        /// </returns>
        /// <response code="200">Success - products removed from the cart.</response>
        /// <response code="400">Bad Request - invalid product IDs.</response>
        /// <response code="401">Unauthorized - API key is missing or invalid.</response>
        /// <response code="500">Internal Server Error - an error occurred while processing the request.</response>
        [HttpDelete]
        [ApiKeyRequired, Authorize(Roles = "Web_User")]
        public Task<IActionResult> DeleteProductsFromCart([FromBody] List<string> products)
        { throw new NotImplementedException(); }
    }
}
