using Asp.Versioning;
using AutoMapper;
using EcommerceAPI.Domain;
using EcommerceAPI.DTOs;
using EcommerceAPI.DTOs.GenericResponse;
using EcommerceAPI.Services.IServices;
using EcommerceAPI.Utilities;
using EcommerceAPI.Utilities.ApplicationRoles;
using EcommerceAPI.Utilities.Exceptions;
using EcommerceAPI.Utilities.Filters;
using EcommerceAPI.Utilities.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrdersController"/> class.
        /// </summary>
        public OrdersController(IOrderServices orderServices, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _orderServices = orderServices;
            _userManager = userManager;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new order.
        /// </summary>
        [HttpPost(""), MapToApiVersion(1.0), Authorize(Roles = $"{ApplicationRoles.WEB_USER}")]
        public async Task<IActionResult> CreateOrder([FromBody] ShippingAddressDTO shippingAddress, [FromHeader(Name = HeaderKeys.PaymentIntentId)] string paymentIntentId)
        {
            var modelState = ModelValidator.ValidateModel(shippingAddress);
            if (!modelState.IsValid) throw new ModelValidationException(modelState);
            var userId = User.GetUserId() ?? throw new UnauthorizedAccessException("User not authenticated.");
            var createdOrder = await _orderServices.CreateOrder(shippingAddress, userId, paymentIntentId);
            return Ok(new { message = "Order successfull.", orderId = createdOrder });
        }

        /// <summary>
        /// Cancels an order by user own if the order's payment is cash on delivery and order status is pending.
        /// </summary>
        [HttpDelete("{orderId}"), MapToApiVersion(1.0), Authorize(Roles = $"{ApplicationRoles.WEB_USER}")]
        public async Task<IActionResult> CancelOrder(string orderId)
        {
            if (string.IsNullOrEmpty(orderId)) return BadRequest(new { Error = "Route value 'order-id' must be given." });
            var userId = User.GetUserId() ?? throw new UnauthorizedAccessException("User not authenticated.");

            // if user select cash on delivery payment method, then order status will be pending by default. In this case, user can cancel the order.
            var order = await _orderServices.GetOrder(orderId, userId);
            if (order == null) return NotFound(new { Error = "Order not found." });
            if (order.OrderStatus == OrdersStatus.Pending.ToString() && order.PaymentStatus == PaymentStatus.Due.ToString() && order.PaymentType == PaymentType.CashOnDelivery.ToString())
            {
                await _orderServices.CancelOrder(order.Id, userId);
                return Ok(new { message = "Order cancelled successfully." });
            }
            return StatusCode(StatusCodes.Status400BadRequest, "This order could not be canceled. Try to take admin support to cancel.");
        }

        /// <summary>
        /// Cancel order by admin.
        /// </summary>
        [Route("/api/admin/v{version:apiVersion}/[controller]"), HttpDelete]
        [MapToApiVersion(2.0), Authorize(Roles = $"{ApplicationRoles.ADMIN}")]
        public async Task<IActionResult> CancelOrderByAdmin([FromQuery] string orderId)
        {
            if (string.IsNullOrEmpty(orderId)) return BadRequest(new { Error = "Route value 'orderid' must be given." });

            // Validate if the order exists
            var order = await _orderServices.GetOrder(orderId: orderId, null);
            if (order == null) return NotFound(new { Error = "Order not found." });

            // Admin can cancel any types of order
            await _orderServices.CancelOrder(orderId, null);
            return Ok(new { message = "Order cancelled successfully." });
        }

        /// <summary>
        /// Retrieves all orders for the current user.
        /// </summary>
        // These 3 lines bellow is for swagger api
        [ProducesResponseType(typeof(GenericResponseDTO<OrderDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet(""), MapToApiVersion(1.0), Authorize(Roles = $"{ApplicationRoles.WEB_USER}")]
        public async Task<IActionResult> GetAllOrdersByUser([FromQuery] SortBy? sortBy = SortBy.DateDESC)
        {
            var userId = User.GetUserId() ?? throw new UnauthorizedAccessException("User not authenticated.");
            var orders = await _orderServices.GetAllOrders(userId: userId, sortBy: sortBy);
            return Ok(new GenericResponseDTO<OrderDTO>
            {
                Data = _mapper.Map<IEnumerable<OrderDTO>>(orders),
                Count = orders?.Count() ?? 0,
            });
        }

        /// <summary>
        /// Retrieves all orders for the users from admin panel.
        /// </summary>
        [Route("/api/admin/v{version:apiVersion}/[controller]")]
        [HttpGet]
        [MapToApiVersion(2.0), Authorize(Roles = $"{ApplicationRoles.ADMIN}")]
        public async Task<IActionResult> GetAllOrdersByAdmin([FromQuery] OrdersStatus? orderStatus, [FromQuery] PaymentStatus? paymentStatus, [FromQuery] PaymentType? paymentType, [FromQuery] SortBy? sortBy = SortBy.DateDESC)
        {
            var orders = await _orderServices.GetAllOrders(userId: null, orderStatus: orderStatus, paymentStatus: paymentStatus, paymentType: paymentType, sortBy: sortBy);
            return Ok(new GenericResponseDTO<OrderDTO>
            {
                Data = _mapper.Map<IEnumerable<OrderDTO>>(orders),
                Count = orders?.Count() ?? 0,
            });
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
            return Ok(_mapper.Map<OrderDTO>(order));
        }

        /// <summary>
        /// Generate and set a tracking number after confirming each order.
        /// </summary>
        [HttpPut("/api/admin/v{version:apiVersion}/[controller]/{orderId}/tracking-id"), MapToApiVersion(2.0), Authorize(Roles = $"{ApplicationRoles.ADMIN}")]
        public async Task<IActionResult> GenerateAndSetOrderTrackingNumber(string orderId)
        {
            await _orderServices.SetOrderTrackingId(orderId);
            return Ok(new { Message = "Tracking ID has been successfully generated and assigned. Order status updated to 'Preparing'." });
        }

        /// <summary>
        /// For Updating order status by admin.
        /// OrderStatus can be updated to Accepted, Preparing, Shipped, Delivered, or Returned.
        /// </summary>
        [HttpPut("/api/admin/v{version:apiVersion}/[controller]/{orderId}/update")]
        [Authorize(Roles = $"{ApplicationRoles.ADMIN}"), MapToApiVersion(2.0)]
        public async Task<IActionResult> UpdateOrder(string orderId, [FromQuery] OrdersStatus? orderStatus, [FromBody] OrderAddressUpdateDTO orderAddress)
        {
            await _orderServices.UpdateOrder(orderId: orderId, order: new OrderUpdateDTO { orderStatus = orderStatus, ShippingAddress = orderAddress });
            return Ok();
        }
    }
}