using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EcommerceAPI.DTOs;
using EcommerceAPI.Services.IServices;
using EcommerceAPI.Utilities.ApplicationRoles;
using EcommerceAPI.Utilities.Filters;
using EcommerceAPI.Utilities.Validation;
using System.Security.Claims;

namespace EcommerceAPI.Api.Controllers
{
    /// <summary>
    /// Controller for managing orders.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderServices _orderServices;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrdersController"/> class.
        /// </summary>
        public OrdersController(IOrderServices orderServices)
        {
            _orderServices = orderServices;
        }

        /// <summary>
        /// Creates a new order.
        /// </summary>
        [HttpPost(""), Authorize(Roles = $"{ApplicationRoles.WEB_USER}"), ApiKeyRequired]
        public async Task<IActionResult> CreateOrder([FromBody] ShippingAddressDTO shippingAddress, [FromQuery] string paymentIntentId)
        {
            var modelState = ModelValidator.ValidateModel(shippingAddress);

            if (!modelState.IsValid)
            {
                var errors = ModelValidator.GetErrors(modelState);
                return BadRequest(new { Errors = errors });
            }
            var userId = (User.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.Actor)?.Value ?? throw new ArgumentNullException("UserId not found.");
            var createdOrder = await _orderServices.CreateOrder(shippingAddress, userId, paymentIntentId);
            return Ok(createdOrder);
        }

        /// <summary>
        /// Cancels an order.
        /// </summary>
        /// <param name="orderId">The ID of the order to cancel.</param>
        [HttpDelete("{orderId}"), Authorize(Roles = ApplicationRoles.WEB_USER), ApiKeyRequired]
        public async Task<IActionResult> CancelOrder(string orderId)
        {
            if (string.IsNullOrEmpty(orderId)) return BadRequest(new { Error = "Route Parameter \"orderId\" must be given." });

            var userId = (User.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.Actor)?.Value ?? throw new ArgumentNullException("User not found.");
            await _orderServices.CancelOrder(orderId, userId);
            return StatusCode(StatusCodes.Status202Accepted, new { Message = "Order successfully cancelled." });
        }

        /// <summary>
        /// Retrieves all orders for the current user.
        /// </summary>
        /// <returns>A response containing the list of orders.</returns>
        [HttpGet(""), Authorize(Roles = ApplicationRoles.WEB_USER), ApiKeyRequired]
        public async Task<IActionResult> GetAllOrders()
        {
            var userId = (User.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.Actor)?.Value ?? throw new ArgumentNullException("User not found.");
            var orders = await _orderServices.GetAllOrders(userId);
            return Ok(orders);
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

            var userId = (User.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.Actor)?.Value ?? throw new ArgumentNullException("User not found.");
            var order = await _orderServices.GetOrder(orderId, userId);
            return Ok();
        }
    }
}