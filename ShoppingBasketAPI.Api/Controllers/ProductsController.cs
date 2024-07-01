using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingBasketAPI.DTOs;
using ShoppingBasketAPI.Services.IServices;
using ShoppingBasketAPI.Utilities;
using ShoppingBasketAPI.Utilities.ApplicationRoles;
using ShoppingBasketAPI.Utilities.Exceptions;
using ShoppingBasketAPI.Utilities.Exceptions.Handler;
using ShoppingBasketAPI.Utilities.Filters;
using ShoppingBasketAPI.Utilities.Validation;

namespace ShoppingBasketAPI.Api.Controllers
{
    /// <summary>
    /// Controller for managing products in the Shopping Basket API.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductServices _productService;
        private readonly ExceptionHandler<ProductsController> _exceptionHandler;

        /// <summary>
        /// Constructor for ProductsController.
        /// </summary>
        /// <param name="productService">The service handling product operations.</param>
        /// <param name="exceptionHandler">Logger instance for logging.</param>
        public ProductsController(IProductServices productService, ExceptionHandler<ProductsController> exceptionHandler)
        {
            this._productService = productService;
            this._exceptionHandler = exceptionHandler;
        }

        /***
         * Get all products.
         */
        /// <summary>
        /// Retrieves all products.
        /// </summary>
        /// <returns>Returns a list of all product.</returns>
        [HttpGet("")]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var productsResult = await _productService.GetAllProduct();
                return Ok(productsResult);
            }
            catch (Exception ex)
            {
                return _exceptionHandler.HandleException(ex, "An error occured while getting all product.");
            }
        }

        /***
         * Get single product by product id.
         */
        /// <summary>
        /// Retrieves a single product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product to retrieve.</param>
        /// <returns>Returns the product with the specified ID.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductsById([FromRoute] string id)
        {
            try
            {
                var productResult = await _productService.GetProductById(id);
                return Ok(productResult);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return _exceptionHandler.HandleException(ex, "An error occured while getting product by id.");
            }
        }

        /***
         * Create product.
         */
        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <param name="productDto">The data for the new product.</param>
        /// <returns>Returns the newly created product.</returns>
        [HttpPost(""), Authorize(Roles = "Admin"), ApiKeyRequired]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateDTO productDto)
        {
            var modelState = ModelValidator.ValidateModel(productDto);
            if (!modelState.IsValid)
            {
                var errors = ModelValidator.GetErrors(modelState);
                return BadRequest(new { Errors = errors });
            }

            try
            {
                var newProduct = await _productService.CreateProduct(productDto);
                return Ok(newProduct);
            }
            catch (Exception ex)
            {
                return _exceptionHandler.HandleException(ex, "An error occured while creating new product.");
            }
        }

        /***
         * Deleting product.
         */
        /// <summary>
        /// Deletes a product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product to delete.</param>
        /// <returns>Returns a success message if deletion is successful.</returns>
        [HttpDelete("{id}"), Authorize(Roles = "Admin"), ApiKeyRequired]
        public async Task<IActionResult> DeleteProduct([FromRoute] string id)
        {
            try
            {
                await _productService.DeleteProduct(id);
                return Ok(new { Message = ResponseMessages.StatusCode_200_DeleteMessage });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return _exceptionHandler.HandleException(ex, "An error occured while deleting product.");
            }
        }

        /***
         * Updating product.
         */
        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="id">The ID of the product to update.</param>
        /// <param name="productDto">The updated data for the product.</param>
        /// <returns>Returns the updated product.</returns>
        [HttpPut("{id}"), Authorize(Roles = "Admin"), ApiKeyRequired]
        public async Task<IActionResult> UpdateProduct(string id, [FromBody] ProductUpdateDTO productDto)
        {
            try
            {
                var product = await _productService.UpdateProduct(id, productDto);
                return Ok(product);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return _exceptionHandler.HandleException(ex, "An error occured while updating the product.");
            }
        }
    }
}