using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ShoppingBasketAPI.Domain;
using ShoppingBasketAPI.DTOs;
using ShoppingBasketAPI.Services.IServices;
using ShoppingBasketAPI.Utilities;
using ShoppingBasketAPI.Utilities.Validation;

namespace ShoppingBasketAPI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private IProductServices _productService;
        private ILogger<ProductsController> _logger;

        public ProductsController(IProductServices productService, ILogger<ProductsController> logger)
        {
            _productService = productService;
            _logger = logger;
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
                return StatusCode(500, new { Error = GlobalErrorMessage.ErrorMessage });
            }
        }

        /***
         * Get single product by product id.
         */

        [HttpGet("id")]
        public async Task<IActionResult> GetProductsById(string id)
        {
            try
            {
                var productResult = await _productService.GetProductById(id);
                return Ok(productResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while getting product by id.");
                return StatusCode(500, new { Error = GlobalErrorMessage.ErrorMessage });
            }
        }

        /***
         * Create product.
         */

        [HttpPost("")]
        public async Task<IActionResult> CreateProduct(ProductCreateRequestDTO productDto)
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
                var newProduct = new Product
                {
                    Name = productDto.Name.Trim(),
                    Description = productDto.Description.Trim(),
                    Price = productDto.Price,
                    Images = productDto.ImageUrls.Select(url => new Image { ImageUrl = url }).ToList(),
                };

                newProduct = await _productService.CreateProduct(newProduct);
                return Ok(newProduct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while creating new product.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { Error = GlobalErrorMessage.ErrorMessage });
            }
        }
    }
}
