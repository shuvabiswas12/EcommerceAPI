using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingBasketAPI.Services.IServices;

namespace ShoppingBasketAPI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private IProductServices _productService;

        public ProductsController(IProductServices productService)
        {
            _productService = productService;
        }

        /***
         * Get all products.
         */

        [HttpGet("")]
        public async Task<IActionResult> GetAllProducts()
        {
            var productsResult = await _productService.GetAllProduct();
            return Ok(productsResult);
        }

        /***
         * Get single product by product id.
         */

        [HttpGet("id")]
        public async Task<IActionResult> GetProductsById(string id)
        {
            var productResult = await _productService.GetProductById(id);
            return Ok(productResult);
        }
    }
}
