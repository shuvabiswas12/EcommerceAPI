using EcommerceAPI.DTOs;
using EcommerceAPI.Services.IServices;
using EcommerceAPI.Utilities;
using EcommerceAPI.Utilities.Exceptions;
using EcommerceAPI.Utilities.Filters;
using EcommerceAPI.Utilities.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Api.Controllers
{
    /// <summary>
    /// Controller for managing products in the Shopping Basket API.
    /// </summary>
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductServices _productService;

        /// <summary>
        /// Constructor for ProductsController.
        /// </summary>
        public ProductsController(IProductServices productService)
        {
            this._productService = productService;
        }

        /// <summary>
        /// Retrieves all products.
        /// </summary>
        /// <returns>Returns a list of all product.</returns>
        [HttpGet("api/[controller]")]
        public async Task<IActionResult> GetAllProducts()
        {
            var productsResult = await _productService.GetAllProduct();
            return Ok(productsResult);
        }

        /// <summary>
        /// Retrieves a single product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product to retrieve.</param>
        /// <returns>Returns the product with the specified ID.</returns>
        [HttpGet("api/[controller]/{id}")]
        public async Task<IActionResult> GetProductById([FromRoute] string id)
        {
            var productResult = await _productService.GetProductById(id);
            return Ok(productResult);
        }

        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <param name="productDto">The data for the new product.</param>
        /// <returns>Returns the newly created product.</returns>
        [HttpPost("api/admin/[controller]"), Authorize(Roles = "Admin"), ApiKeyRequired]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateDTO productDto)
        {
            var modelState = ModelValidator.ValidateModel(productDto);
            if (!modelState.IsValid)
            {
                throw new ModelValidationException(modelState);
            }

            var newProduct = await _productService.CreateProduct(productDto);
            return CreatedAtAction(nameof(GetProductById), new { id = newProduct.Id });
        }

        /// <summary>
        /// Deletes a product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product to delete.</param>
        /// <returns>Returns a success message if deletion is successful.</returns>
        [HttpDelete("api/admin/[controller]/{id}"), Authorize(Roles = "Admin"), ApiKeyRequired]
        public async Task<IActionResult> DeleteProduct([FromRoute] string id)
        {
            await _productService.DeleteProduct(id);
            return NoContent();
        }

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="id">The ID of the product to update.</param>
        /// <param name="productDto">The updated data for the product.</param>
        /// <returns>Returns the updated product.</returns>
        [HttpPut("api/admin/[controller]/{id}"), Authorize(Roles = "Admin"), ApiKeyRequired]
        public async Task<IActionResult> UpdateProduct(string id, [FromBody] ProductUpdateDTO productDto)
        {
            var product = await _productService.UpdateProduct(id, productDto);
            return Ok(product);
        }
    }
}