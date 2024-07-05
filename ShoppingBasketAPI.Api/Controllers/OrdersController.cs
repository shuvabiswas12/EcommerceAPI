using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingBasketAPI.DTOs;
using ShoppingBasketAPI.Services.IServices;
using ShoppingBasketAPI.Utilities.ApplicationRoles;
using ShoppingBasketAPI.Utilities.Exceptions.Handler;
using ShoppingBasketAPI.Utilities.Filters;
using ShoppingBasketAPI.Utilities.Validation;
using System.Security.Claims;

namespace ShoppingBasketAPI.Api.Controllers
{
    /// <summary>
    /// Controller for managing orders.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderServices _orderServices;
        private readonly ExceptionHandler<OrdersController> _exceptionHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrdersController"/> class.
        /// </summary>
        /// <param name="orderServices">The service for managing orders.</param>
        /// <param name="exceptionHandler">The exception handler for the controller.</param>
        public OrdersController(IOrderServices orderServices, ExceptionHandler<OrdersController> exceptionHandler)
        {
            _orderServices = orderServices;
            _exceptionHandler = exceptionHandler;
        }

        /// <summary>
        /// Creates a new order.
        /// </summary>
        /// <param name="shippingAddress">The shipping address for the order.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        [HttpPost(""), Authorize(Roles = $"{ApplicationRoles.WEB_USER}"), ApiKeyRequired]
        public async Task<IActionResult> CreateOrder([FromBody] ShippingAddressDTO shippingAddress, [FromQuery] string paymentIntentId)
        {
            var modelState = ModelValidator.ValidateModel(shippingAddress);

            if (!modelState.IsValid)
            {
                var errors = ModelValidator.GetErrors(modelState);
                return BadRequest(new { Errors = errors });
            }
            try
            {
                var userId = (User.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.Actor)?.Value ?? throw new ArgumentNullException("UserId not found.");
                var createdOrder = await _orderServices.CreateOrder(shippingAddress, userId, paymentIntentId);
                return Ok(createdOrder);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return _exceptionHandler.HandleException(ex, "There is an error while creating the order.");
            }
        }

        /// <summary>
        /// Cancels an order.
        /// </summary>
        /// <param name="orderId">The ID of the order to cancel.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        [HttpDelete("{orderId}"), Authorize(Roles = ApplicationRoles.WEB_USER), ApiKeyRequired]
        public async Task<IActionResult> CancelOrder(string orderId)
        {
            if (string.IsNullOrEmpty(orderId)) return BadRequest(new { Error = "Route Parameter \"orderId\" must be given." });


            try
            {
                var userId = (User.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.Actor)?.Value ?? throw new ArgumentNullException("User not found.");
                await _orderServices.CancelOrder(orderId, userId);
                return StatusCode(StatusCodes.Status202Accepted, new { Message = "Order successfully cancelled." });
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return _exceptionHandler.HandleException(ex, "There is an error occured while cancelling the order.");
            }
        }

        /// <summary>
        /// Retrieves all orders for the current user.
        /// </summary>
        /// <returns>A response containing the list of orders.</returns>
        [HttpGet(""), Authorize(Roles = ApplicationRoles.WEB_USER), ApiKeyRequired]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                var userId = (User.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.Actor)?.Value ?? throw new ArgumentNullException("User not found.");
                var orders = await _orderServices.GetAllOrders(userId);
                return Ok(orders);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return _exceptionHandler.HandleException(ex, "There is an error occured while getting all orders.");
            }
        }

        /// <summary>
        /// Retrieves an order by its ID.
        /// </summary>
        /// <param name="orderId">The ID of the order to retrieve.</param>
        /// <returns>A response containing the order details.</returns>
        [HttpGet("{orderId}"), Authorize(Roles = ApplicationRoles.WEB_USER), ApiKeyRequired]
        public async Task<IActionResult> GetOrderByOrderId(string orderId)
        {
            if (string.IsNullOrEmpty(orderId)) return BadRequest(new { Error = "Route Parameter \"orderId\" must be given." });

            try
            {
                var userId = (User.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.Actor)?.Value ?? throw new ArgumentNullException("User not found.");
                var order = await _orderServices.GetOrder(orderId, userId);
                return Ok();
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return _exceptionHandler.HandleException(ex, "An error occured while getting the order.");
            }
        }
    }
}
