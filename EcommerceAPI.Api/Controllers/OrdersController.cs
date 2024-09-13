using EcommerceAPI.DTOs;
using EcommerceAPI.Services.IServices;
using EcommerceAPI.Utilities;
using EcommerceAPI.Utilities.ApplicationRoles;
using EcommerceAPI.Utilities.Exceptions;
using EcommerceAPI.Utilities.Filters;
using EcommerceAPI.Utilities.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Api.Controllers
{
    /// <summary>
    /// Controller for managing orders.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = $"{ApplicationRoles.WEB_USER}"), ApiKeyRequired]
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
        [HttpPost("")]
        public async Task<IActionResult> CreateOrder([FromBody] ShippingAddressDTO shippingAddress, [FromQuery] string paymentIntentId)
        {
            var modelState = ModelValidator.ValidateModel(shippingAddress);
            if (!modelState.IsValid) throw new ModelValidationException(modelState);
            var userId = User.GetUserId() ?? throw new UnauthorizedAccessException("User not authenticated.");
            var createdOrder = await _orderServices.CreateOrder(shippingAddress, userId, paymentIntentId);
            return CreatedAtAction(nameof(GetOrderByOrderId), new { orderId = createdOrder.Id });
        }

        /// <summary>
        /// Cancels an order.
        /// </summary>
        /// <param name="orderId">The ID of the order to cancel.</param>
        [HttpDelete("{orderId}")]
        public async Task<IActionResult> CancelOrder(string orderId)
        {
            if (string.IsNullOrEmpty(orderId)) return BadRequest(new { Error = "Route value 'order-id' must be given." });
            var userId = User.GetUserId() ?? throw new ArgumentNullException("User not authenticated.");
            await _orderServices.CancelOrder(orderId, userId);
            return NoContent();
        }

        /// <summary>
        /// Retrieves all orders for the current user.
        /// </summary>
        /// <returns>A response containing the list of orders.</returns>
        [HttpGet("")]
        public async Task<IActionResult> GetAllOrders()
        {
            var userId = User.GetUserId() ?? throw new ArgumentNullException("User not authenticated.");
            var orders = await _orderServices.GetAllOrders(userId);
            return Ok(orders);
        }

        /// <summary>
        /// Retrieves an order by its ID.
        /// </summary>
        /// <param name="orderId">The ID of the order to retrieve.</param>
        /// <returns>A response containing the order details.</returns>
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderByOrderId(string orderId)
        {
            if (string.IsNullOrEmpty(orderId)) return BadRequest(new { Error = "Route value 'order-id' must be given." });
            var userId = User.GetUserId() ?? throw new UnauthorizedAccessException("User not authenticated.");
            var order = await _orderServices.GetOrder(orderId, userId);
            return Ok(order);
        }
    }
}