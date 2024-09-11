using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EcommerceAPI.Domain;
using EcommerceAPI.DTOs;
using EcommerceAPI.Services.IServices;
using EcommerceAPI.Utilities;
using EcommerceAPI.Utilities.ApplicationRoles;
using EcommerceAPI.Utilities.Filters;

namespace EcommerceAPI.Api.Controllers
{
    /// <summary>
    /// Controller for managing categories in the Shopping Basket API.
    /// </summary>
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryServices _categoryServices;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoriesController"/> class.
        /// </summary>
        public CategoriesController(ICategoryServices categoryServices)
        {
            this._categoryServices = categoryServices;
        }

        /// <summary>
        /// Gets all categories.
        /// </summary>
        /// <returns>A list of all categories.</returns>
        [HttpGet("api/[controller]")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _categoryServices.GetAllCategories();
            return Ok(result);
        }

        /// <summary>
        /// Creates a new category.
        /// </summary>
        /// <param name="createCategory">The category to create.</param>
        /// <returns>The created category.</returns>
        [HttpPost("api/admin/[controller]"), Authorize(Roles = ApplicationRoles.ADMIN), ApiKeyRequired]
        public async Task<IActionResult> CreateCategory(CategoryCreateDTO createCategory)
        {
            var category = await _categoryServices.CreateCategory(createCategory);
            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
        }

        /// <summary>
        /// Updates an existing category.
        /// </summary>
        /// <param name="categoryToUpdate">The category to update.</param>
        /// <returns>The updated category.</returns>
        [HttpPut("api/admin/[controller]"), Authorize(Roles = ApplicationRoles.ADMIN), ApiKeyRequired]
        public async Task<IActionResult> UpdateCategory(CategoryUpdateDTO categoryToUpdate)
        {
            var updatedCategory = await _categoryServices.UpdateCategory(categoryToUpdate.Id.Trim(), categoryToUpdate.Name.Trim());
            return Ok(updatedCategory);
        }

        /// <summary>
        /// Gets a single category by ID.
        /// </summary>
        /// <param name="id">The category ID.</param>
        /// <returns>The category with the specified ID.</returns>
        [HttpGet("api/[controller]/{id}")]
        public async Task<IActionResult> GetCategoryById(string id)
        {
            var result = await _categoryServices.GetCategoryById(id);
            return Ok(result);
        }

        /// <summary>
        /// Deletes a category by ID.
        /// </summary>
        /// <param name="id">The category ID.</param>
        /// <returns>A message indicating the result of the deletion.</returns>
        [HttpDelete("api/admin/[controller]/{id}"), Authorize(Roles = ApplicationRoles.ADMIN), ApiKeyRequired]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            await _categoryServices.DeleteCategory(id);
            return NoContent();
        }
    }
}