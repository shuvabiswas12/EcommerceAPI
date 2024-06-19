using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingBasketAPI.DTOs;
using ShoppingBasketAPI.Services.IServices;
using ShoppingBasketAPI.Utilities;
using ShoppingBasketAPI.Utilities.Exceptions;
using ShoppingBasketAPI.Utilities.Validation;

namespace ShoppingBasketAPI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductServices _productService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductServices productService, ILogger<ProductsController> logger)
        {
            this._productService = productService;
            this._logger = logger;
        }

        /***
         * Get all products.
         */

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
                _logger.LogError(ex, "An error occured while getting all product.");
                return StatusCode(500, new { Error = ResponseMessages.StatusCode_500_ErrorMessage });
            }
        }

        /***
         * Get single product by product id.
         */

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductsById([FromRoute] string id)
        {
            try
            {
                var productResult = await _productService.GetProductById(id);
                return Ok(productResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while getting product by id.");
                return StatusCode(500, new { Error = ResponseMessages.StatusCode_500_ErrorMessage });
            }
        }

        /***
         * Create product.
         */

        [HttpPost("")]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateRequestDTO productDto)
        {
            var modelState = ModelValidator.ValidateModel(productDto);
            if (!modelState.IsValid)
            {
                var errors = modelState
                    .Where(ms => ms.Value!.Errors.Any())
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                return BadRequest(new { Errors = errors });
            }

            try
            {
                var newProduct = await _productService.CreateProduct(productDto);
                return Ok(newProduct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while creating new product.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { Error = ResponseMessages.StatusCode_500_ErrorMessage });
            }
        }

        /***
         * Deleting product.
         */

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] string id)
        {
            try
            {
                await _productService.DeleteProduct(id);
                return Ok(new { Message = ResponseMessages.StatusCode_200_DeleteMessage });
            }
            catch (NotFoundException ex)
            {
                _logger.LogError(ex, ex.Message);
                return NotFound(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while deleting product.");
                return StatusCode(500, new { Error = ResponseMessages.StatusCode_500_ErrorMessage });
            }
        }

        /***
         * Updating product.
         */

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(string id, [FromBody] ProductUpdateRequestDTO productDto)
        {
            try
            {
                var product = await _productService.UpdateProduct(id, productDto);
                return Ok(product);
            }
            catch (NotFoundException ex)
            {
                _logger.LogError(ex, ex.Message);
                return NotFound(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while updating the product.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { Error = ResponseMessages.StatusCode_500_ErrorMessage });
            }
        }
    }
}