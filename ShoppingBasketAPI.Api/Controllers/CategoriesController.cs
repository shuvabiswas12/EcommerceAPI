using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingBasketAPI.Domain;
using ShoppingBasketAPI.DTOs;
using ShoppingBasketAPI.Services.IServices;
using ShoppingBasketAPI.Utilities;
using ShoppingBasketAPI.Utilities.Exceptions;
using ShoppingBasketAPI.Utilities.Exceptions.Handler;
using ShoppingBasketAPI.Utilities.Filters;

namespace ShoppingBasketAPI.Api.Controllers
{
    /// <summary>
    /// Controller for managing categories in the Shopping Basket API.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryServices _categoryServices;
        private readonly ExceptionHandler<CategoriesController> _exceptionHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoriesController"/> class.
        /// </summary>
        /// <param name="categoryServices">The category services.</param>
        /// <param name="exceptionHandler">The exception handler and error logger.</param>
        public CategoriesController(ICategoryServices categoryServices, ExceptionHandler<CategoriesController> exceptionHandler)
        {
            this._categoryServices = categoryServices;
            this._exceptionHandler = exceptionHandler;
        }

        /***
         * Get all category.
         */
        /// <summary>
        /// Gets all categories.
        /// </summary>
        /// <returns>A list of all categories.</returns>
        /// <response code="200">Returns a list of all categories.</response>
        /// <response code="500">If an error occurs while getting the categories.</response>
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
                return _exceptionHandler.HandleException(ex, "An error occured while getting all category.");
            }
        }

        /***
         * Create category
         */
        /// <summary>
        /// Creates a new category.
        /// </summary>
        /// <param name="createCategory">The category to create.</param>
        /// <returns>The created category.</returns>
        /// <response code="200">Returns the created category.</response>
        /// <response code="400">If the category name is missing or empty.</response>
        /// <response code="500">If an error occurs while creating the category.</response>
        [HttpPost(""), Authorize(Roles = "Admin"), ApiKeyRequired]
        public async Task<IActionResult> CreateCategory(CategoryCreateDTO createCategory)
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
                return _exceptionHandler.HandleException(ex, "An error occured while creating category.");
            }
        }

        /***
         * Category update
         */
        /// <summary>
        /// Updates an existing category.
        /// </summary>
        /// <param name="categoryToUpdate">The category to update.</param>
        /// <returns>The updated category.</returns>
        /// <response code="200">Returns the updated category.</response>
        /// <response code="400">If the category name or ID is missing or empty.</response>
        /// <response code="404">If the category is not found.</response>
        /// <response code="500">If an error occurs while updating the category.</response>
        [HttpPut(""), Authorize(Roles = "Admin"), ApiKeyRequired]
        public async Task<IActionResult> UpdateCategory(CategoryUpdateDTO categoryToUpdate)
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
                return NotFound(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return _exceptionHandler.HandleException(ex, "An error occured while updating category.");
            }
        }

        /***
         * Get a single category.
         */
        /// <summary>
        /// Gets a single category by ID.
        /// </summary>
        /// <param name="id">The category ID.</param>
        /// <returns>The category with the specified ID.</returns>
        /// <response code="200">Returns the category with the specified ID.</response>
        /// <response code="404">If the category is not found.</response>
        /// <response code="500">If an error occurs while getting the category.</response>
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
                return NotFound(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return _exceptionHandler.HandleException(ex, "An error occured while getting category.");
            }
        }

        /***
         * Delete category
         */
        /// <summary>
        /// Deletes a category by ID.
        /// </summary>
        /// <param name="id">The category ID.</param>
        /// <returns>A message indicating the result of the deletion.</returns>
        /// <response code="200">If the category was successfully deleted.</response>
        /// <response code="400">If the category ID is missing or empty.</response>
        /// <response code="404">If the category is not found.</response>
        /// <response code="500">If an error occurs while deleting the category.</response>
        [HttpDelete("{id}"), Authorize(Roles = "Admin"), ApiKeyRequired]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            if (id.Trim().Length == 0)
            {
                return BadRequest(new { Error = "Missing category id." });
            }
            try
            {
                await _categoryServices.DeleteCategory(id);
                return Ok(new { Message = ResponseMessages.StatusCode_200_DeleteMessage });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return _exceptionHandler.HandleException(ex, "An error occured while getting category.");
            }
        }
    }
}