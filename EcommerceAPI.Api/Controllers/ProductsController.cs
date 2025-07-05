using Asp.Versioning;
using AutoMapper;
using EcommerceAPI.DTOs;
using EcommerceAPI.DTOs.GenericResponse;
using EcommerceAPI.Services.IServices;
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
    [ApiVersion(1.0), ApiVersion(2.0)]
    public class ProductsController : ControllerBase
    {
        private readonly IProductServices _productService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor for ProductsController.
        /// </summary>
        public ProductsController(IProductServices productService, IMapper mapper)
        {
            this._productService = productService;
            this._mapper = mapper;
        }

        /// <summary>
        /// Retrieves all products.
        /// </summary>
        [MapToApiVersion(1.0)]
        [HttpGet("api/v{version:apiVersion}/[controller]")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProduct();
            return Ok(new GenericResponseDTO<ProductDTO>
            {
                Data = _mapper.Map<IEnumerable<ProductDTO>>(products),
                Count = products.Count()
            });
        }

        /// <summary>
        /// Retrieves a single product by its ID.
        /// </summary>
        [MapToApiVersion(1.0)]
        [HttpGet("api/v{version:apiVersion}/[controller]/{id}")]
        public async Task<IActionResult> GetProductById([FromRoute] string id)
        {
            var product = await _productService.GetProductById(id);
            return Ok(_mapper.Map<ProductDTO>(product));
        }

        /// <summary>
        /// Creates a new product.
        /// </summary>
        [MapToApiVersion(2.0)]
        [HttpPost("api/admin/v{version:apiVersion}/[controller]"), Authorize(Roles = "Admin"), ApiKeyRequired]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateDTO productDto)
        {
            var modelState = ModelValidator.ValidateModel(productDto);
            if (!modelState.IsValid)
            {
                throw new ModelValidationException(modelState);
            }

            var newProduct = await _productService.CreateProduct(productDto);
            return StatusCode(StatusCodes.Status201Created, new { Id = newProduct });
        }

        /// <summary>
        /// Deletes a product by its ID.
        /// </summary>
        [MapToApiVersion(2.0)]
        [HttpDelete("api/admin/v{version:apiVersion}/[controller]/{id}"), Authorize(Roles = "Admin"), ApiKeyRequired]
        public async Task<IActionResult> DeleteProduct([FromRoute] string id)
        {
            await _productService.DeleteProduct(id);
            return NoContent();
        }

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        [MapToApiVersion(2.0)]
        [HttpPut("api/admin/v{version:apiVersion}/[controller]/{id}"), Authorize(Roles = "Admin"), ApiKeyRequired]
        public async Task<IActionResult> UpdateProduct(string id, [FromBody] ProductUpdateDTO productDto)
        {
            await _productService.UpdateProduct(id, productDto);
            return NoContent();
        }
    }
}