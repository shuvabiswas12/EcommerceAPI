using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EcommerceAPI.DTOs;
using EcommerceAPI.Services.IServices;
using EcommerceAPI.Utilities.ApplicationRoles;
using EcommerceAPI.Utilities.Filters;
using EcommerceAPI.Utilities.Validation.CustomAttributes;
using Stripe;
using System.Security.Claims;

namespace EcommerceAPI.Api.Controllers
{
    /// <summary>
    /// Controller to handle payment-related operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentServices _paymentServices;
        private readonly IShoppingCartServices _shoppingCartServices;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentsController"/> class.
        /// </summary>
        public PaymentsController(IPaymentServices paymentServices, IShoppingCartServices shoppingCartServices)
        {
            _paymentServices = paymentServices;
            _shoppingCartServices = shoppingCartServices;
        }

        /// <summary>
        /// Creates a payment intent and returns the client secret.
        /// </summary>
        /// <returns>Client secret for the payment intent.</returns>
        [HttpPost("payment-intent"), ApiKeyRequired, Authorize(Roles = $"{ApplicationRoles.WEB_USER}")]
        public async Task<IActionResult> CreatePaymentIntent()
        {
            var userId = (User.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.Actor)?.Value;

            if (userId == null) return BadRequest("User not found.");

            var carts = await _shoppingCartServices.GetShoppingCartsByUserId(userId);

            if (carts.TotalCost <= 0) return NotFound(new { Error = "Cart not found." });

            long amount = (long)carts.TotalCost;

            var clientSecret = await _paymentServices.CreatePaymentIntentAsync(amount);
            return Ok(new { clientSecret = clientSecret });
        }
    }
}