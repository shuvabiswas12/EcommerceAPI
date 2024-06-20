using Microsoft.AspNetCore.Mvc;
using ShoppingBasketAPI.DTOs;
using ShoppingBasketAPI.Services.IServices;
using ShoppingBasketAPI.Utilities;
using ShoppingBasketAPI.Utilities.Exceptions;

namespace ShoppingBasketAPI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryServices _categoryServices;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(ICategoryServices categoryServices, ILogger<CategoriesController> logger)
        {
            this._categoryServices = categoryServices;
            this._logger = logger;
        }

        /***
         * Get all category.
         */

        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _categoryServices.GetAllCategories();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while getting all category.");
                return StatusCode(500, new { Error = ResponseMessages.StatusCode_500_ErrorMessage });
            }
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
            try
            {
                var category = await _categoryServices.CreateCategory(createCategory);
                return Ok(new { category });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while creating category.");
                return StatusCode(500, new { Error = ResponseMessages.StatusCode_500_ErrorMessage });
            }
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
                var updatedCategory = await _categoryServices.UpdateCategory(categoryToUpdate.Id.Trim(), categoryToUpdate.Name.Trim());
                return Ok(updatedCategory);
            }
            catch (NotFoundException ex)
            {
                _logger.LogError(ex, ex.Message);
                return NotFound(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while updating category.");
                return StatusCode(500, new { Error = ResponseMessages.StatusCode_500_ErrorMessage });
            }
        }

        /***
         * Get a single category.
         */

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(string id)
        {
            try
            {
                var result = await _categoryServices.GetCategoryById(id);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                _logger.LogError(ex, ex.Message);
                return NotFound(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while getting category.");
                return StatusCode(500, new { Error = ResponseMessages.StatusCode_500_ErrorMessage });
            }
        }

        /***
         * Delete category
         */

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            if (id.Trim().Length == 0)
            {
                return BadRequest(new { Error = "missing category id." });
            }
            try
            {
                await _categoryServices.DeleteCategory(id);
                return Ok(new { Message = ResponseMessages.StatusCode_200_DeleteMessage });
            }
            catch (NotFoundException ex)
            {
                _logger.LogError(ex, ex.Message);
                return NotFound(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while deleting category.");
                return StatusCode(500, new { Error = ResponseMessages.StatusCode_500_ErrorMessage });
            }
        }
    }
}