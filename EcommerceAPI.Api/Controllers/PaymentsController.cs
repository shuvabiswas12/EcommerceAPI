﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EcommerceAPI.DTOs;
using EcommerceAPI.Services.IServices;
using EcommerceAPI.Utilities.ApplicationRoles;
using EcommerceAPI.Utilities.Filters;
using EcommerceAPI.Utilities.Validation.CustomAttributes;
using Stripe;
using System.Security.Claims;
using EcommerceAPI.Utilities;
using Asp.Versioning;

namespace EcommerceAPI.Api.Controllers
{
    /// <summary>
    /// Controller to handle payment-related operations.
    /// </summary>
    [Route("api/v{version:apiVersion}/[controller]"), ApiVersion(1.0)]
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
        [HttpPost("payment-intent"), ApiKeyRequired, Authorize(Roles = $"{ApplicationRoles.WEB_USER}")]
        public async Task<IActionResult> CreatePaymentIntent()
        {
            var userId = User.GetUserId();

            if (userId == null) throw new UnauthorizedAccessException("User not authenticated.");

            var carts = await _shoppingCartServices.GetShoppingCartsByUserId(userId);
            var totalCost = carts.Sum(cart => cart.Product?.Price * cart.Count ?? 0);

            if (carts.Count() <= 0) return NotFound(new { Error = "Cart is empty." });

            long amount = (long)Math.Round(totalCost * 100m);

            var paymentIntent = await _paymentServices.CreatePaymentIntentAsync(amount);
            return StatusCode(StatusCodes.Status201Created, paymentIntent);
        }
    }
}