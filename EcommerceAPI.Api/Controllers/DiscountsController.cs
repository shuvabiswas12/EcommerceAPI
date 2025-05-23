﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EcommerceAPI.DTOs;
using EcommerceAPI.Services.IServices;
using EcommerceAPI.Utilities;
using EcommerceAPI.Utilities.Filters;
using EcommerceAPI.Utilities.Validation;
using EcommerceAPI.Utilities.Exceptions;
using System.ComponentModel.DataAnnotations;
using Asp.Versioning;

namespace EcommerceAPI.Api.Controllers
{
    /// <summary>
    /// Controller for managing discounts related to products in the Shopping Basket API.
    /// </summary>
    [ApiVersion(2.0), Route("api/admin/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class DiscountsController : ControllerBase
    {
        private readonly IDiscountServices _discountServices;

        /// <summary>
        /// Constructor for DiscountsController.
        /// </summary>
        public DiscountsController(IDiscountServices discountServices)
        {
            _discountServices = discountServices;
        }

        /// <summary>
        /// Sets a discount for a product identified by its ID.
        /// </summary>
        [HttpPost("{id}"), ApiKeyRequired, Authorize(Roles = "Admin")]
        public async Task<IActionResult> SetProductDiscount([FromRoute] string id, [FromBody] DiscountRequestDTO discountRequestDTO)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("Product ID must be provided.");
            }

            var modelState = ModelValidator.ValidateModel(discountRequestDTO);
            if (!modelState.IsValid)
            {
                throw new ModelValidationException(modelState);
            }
            await _discountServices.AddDiscount(id, discountRequestDTO);
            return StatusCode(StatusCodes.Status201Created);
        }

        /// <summary>
        /// Removes the discount for a product identified by its ID.
        /// </summary>
        [HttpDelete("{id}"), ApiKeyRequired, Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveProductDiscount([FromRoute] string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("Product ID must be provided.");
            }
            await _discountServices.RemoveDiscount(id);
            return NoContent();
        }
    }
}