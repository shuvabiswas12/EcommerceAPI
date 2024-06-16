using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingBasketAPI.Domain;
using ShoppingBasketAPI.DTOs;
using ShoppingBasketAPI.Services.IServices;

namespace ShoppingBasketAPI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryServices _categoryServices;

        public CategoriesController(ICategoryServices categoryServices)
        {
            this._categoryServices = categoryServices;
        }

        /***
         * Get all category.
         */

        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _categoryServices.GetAllCategories();
            return Ok(result);
        }

        /***
         * Create category
         */

        [HttpPost("")]
        public async Task<IActionResult> CreateCategory(CategoryCreateRequestDTO createCategory)
        {
            if (createCategory.Name.Trim().Length == 0)
            {
                return BadRequest(new { error = "Must be filled with a category name." });
            }
            Category category = await _categoryServices.CreateCategory(new Category { Name = createCategory.Name.Trim() });
            return Ok(new { category });
        }

        /***
         * Category update
         */

        [HttpPut("")]
        public async Task<IActionResult> UpdateCategory(CategoryUpdateRequestDTO categoryToUpdate)
        {
            if (categoryToUpdate.Name.Trim().Length == 0 || categoryToUpdate.Id.Trim().Length == 0)
            {
                return BadRequest(new { error = "Data is missing!" });
            }
            try
            {
                Category updatedCategory = await _categoryServices.UpdateCategory(categoryToUpdate.Id.Trim(), categoryToUpdate.Name.Trim());
                return Ok(updatedCategory);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /***
         * Get a single category.
         */

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _categoryServices.GetCategoryById(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        /***
         * Delete category
         */

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            if (id.Trim().Length == 0)
            {
                return BadRequest(new { error = "missing category id." });
            }
            try
            {
                await _categoryServices.DeleteCategory(id);
                return Ok(new { message = "Deleted." });
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}