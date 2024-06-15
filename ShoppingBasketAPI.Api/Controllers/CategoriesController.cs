using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    }
}