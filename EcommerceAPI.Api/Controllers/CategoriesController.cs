using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EcommerceAPI.Domain;
using EcommerceAPI.DTOs;
using EcommerceAPI.Services.IServices;
using EcommerceAPI.Utilities;
using EcommerceAPI.Utilities.ApplicationRoles;
using EcommerceAPI.Utilities.Filters;
using Asp.Versioning;
using EcommerceAPI.Utilities.Validation;
using EcommerceAPI.Utilities.Exceptions;
using EcommerceAPI.DTOs.GenericResponse;
using AutoMapper;

namespace EcommerceAPI.Api.Controllers
{
    /// <summary>
    /// Controller for managing categories in the Shopping Basket API.
    /// </summary>
    [ApiController, ApiVersion(1.0), ApiVersion(2.0)]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryServices _categoryServices;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoriesController"/> class.
        /// </summary>
        public CategoriesController(ICategoryServices categoryServices, IMapper mapper)
        {
            this._categoryServices = categoryServices;
            this._mapper = mapper;
        }

        /// <summary>
        /// Gets all categories.
        /// </summary>
        [MapToApiVersion(1.0)]
        [HttpGet("api/v{version:apiVersion}/[controller]")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _categoryServices.GetAllCategories();
            return Ok(new GenericResponseDTO<CategoryDTO>
            {
                Data = _mapper.Map<IEnumerable<CategoryDTO>>(result),
                Count = result.Count()
            });
        }

        /// <summary>
        /// Creates a new category.
        /// </summary>
        [MapToApiVersion(2.0)]
        [HttpPost("api/admin/v{version:apiVersion}/[controller]"), Authorize(Roles = ApplicationRoles.ADMIN), ApiKeyRequired]
        public async Task<IActionResult> CreateCategory(CategoryCreateDTO categoryDto)
        {
            var modelState = ModelValidator.ValidateModel(categoryDto);
            if (!modelState.IsValid)
            {
                throw new ModelValidationException(modelState);
            }
            var categoryId = await _categoryServices.CreateCategory(categoryDto);
            return StatusCode(StatusCodes.Status201Created, new { Id = categoryId });
        }

        /// <summary>
        /// Updates an existing category.
        /// </summary>
        [MapToApiVersion(2.0)]
        [HttpPut("api/admin/v{version:apiVersion}/[controller]"), Authorize(Roles = ApplicationRoles.ADMIN), ApiKeyRequired]
        public async Task<IActionResult> UpdateCategory(CategoryUpdateDTO categoryToUpdate)
        {
            await _categoryServices.UpdateCategory(categoryToUpdate.Id.Trim(), categoryToUpdate.Name.Trim());
            return NoContent();
        }

        /// <summary>
        /// Gets a single category by ID.
        /// </summary>
        [MapToApiVersion(1.0)]
        [HttpGet("api/v{version:apiVersion}/[controller]/{id}")]
        public async Task<IActionResult> GetCategoryById(string id)
        {
            var result = await _categoryServices.GetCategoryById(id);
            return Ok(_mapper.Map<CategoryDTO>(result));
        }

        /// <summary>
        /// Deletes a category by ID.
        /// </summary>
        [MapToApiVersion(2.0)]
        [HttpDelete("api/admin/v{version:apiVersion}/[controller]/{id}"), Authorize(Roles = ApplicationRoles.ADMIN), ApiKeyRequired]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            await _categoryServices.DeleteCategory(id);
            return NoContent();
        }
    }
}