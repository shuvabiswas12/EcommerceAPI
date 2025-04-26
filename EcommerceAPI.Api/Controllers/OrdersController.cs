using Asp.Versioning;
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
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController, ApiVersion(1.0), ApiVersion(2.0)]
    [ApiKeyRequired]
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
        [HttpPost(""), MapToApiVersion(1.0), Authorize(Roles = $"{ApplicationRoles.WEB_USER}")]
        public async Task<IActionResult> CreateOrder([FromBody] ShippingAddressDTO shippingAddress, [FromQuery] string paymentIntentId)
        {
            var modelState = ModelValidator.ValidateModel(shippingAddress);
            if (!modelState.IsValid) throw new ModelValidationException(modelState);
            var userId = User.GetUserId() ?? throw new UnauthorizedAccessException("User not authenticated.");
            var createdOrder = await _orderServices.CreateOrder(shippingAddress, userId, paymentIntentId);
            return Ok(createdOrder.Id);
        }

        /// <summary>
        /// Cancels an order.
        /// </summary>
        [HttpDelete("{orderId}"), MapToApiVersion(1.0), Authorize(Roles = $"{ApplicationRoles.WEB_USER}")]
        public async Task<IActionResult> CancelOrder(string orderId)
        {
            if (string.IsNullOrEmpty(orderId)) return BadRequest(new { Error = "Route value 'order-id' must be given." });
            var userId = User.GetUserId() ?? throw new UnauthorizedAccessException("User not authenticated.");
            await _orderServices.CancelOrder(orderId, userId);
            return NoContent();
        }

        /// <summary>
        /// Cancelled order by admin.
        /// </summary>
        [Route("/api/admin/v{version:apiVersion}/[controller]"), HttpDelete]
        [MapToApiVersion(2.0), Authorize(Roles = $"{ApplicationRoles.ADMIN}")]
        public async Task<IActionResult> CancelOrderByAdmin([FromQuery] string orderId, [FromQuery] string userId)
        {
            if (string.IsNullOrEmpty(orderId)) return BadRequest(new { Error = "Route value 'orderid' must be given." });
            if (string.IsNullOrEmpty(userId)) return BadRequest(new { Error = "Route value 'userid' must be given." });
            await _orderServices.CancelOrder(orderId, userId);
            return NoContent();
        }

        /// <summary>
        /// Retrieves all orders for the current user.
        /// </summary>
        [HttpGet(""), MapToApiVersion(1.0), Authorize(Roles = $"{ApplicationRoles.WEB_USER}")]
        public async Task<IActionResult> GetAllOrdersByUser()
        {
            var userId = User.GetUserId() ?? throw new UnauthorizedAccessException("User not authenticated.");
            var orders = await _orderServices.GetAllOrders(userId);
            return Ok(orders);
        }

        /// <summary>
        /// Retrieves all orders for the users from admin panel.
        /// </summary>
        [Route("/api/admin/v{version:apiVersion}/[controller]")]
        [HttpGet]
        [MapToApiVersion(2.0), Authorize(Roles = $"{ApplicationRoles.ADMIN}")]
        public async Task<IActionResult> GetAllOrdersByAdmin()
        {
            var orders = await _orderServices.GetAllOrders();
            return Ok(orders);
        }

        /// <summary>
        /// Retrieves an order by its ID.
        /// </summary>
        [HttpGet("{orderId}"), MapToApiVersion(1.0), Authorize(Roles = $"{ApplicationRoles.ADMIN}, {ApplicationRoles.WEB_USER}")]
        public async Task<IActionResult> GetOrderByOrderId(string orderId)
        {
            if (string.IsNullOrEmpty(orderId)) return BadRequest(new { Error = "Route value 'order-id' must be given." });
            var userId = User.GetUserId() ?? throw new UnauthorizedAccessException("User not authenticated.");
            var order = await _orderServices.GetOrder(orderId, userId);
            return Ok(order);
        }
    }
}