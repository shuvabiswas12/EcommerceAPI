using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ShoppingBasketAPI.Utilities.ApplicationRoles;
using ShoppingBasketAPI.Utilities.Exceptions.Handler;
using ShoppingBasketAPI.Utilities.Filters;

namespace ShoppingBasketAPI.Api.Controllers.Admin
{
    /// <summary>
    /// Controller for managing dashboard-related operations.
    /// </summary>
    [Route("api/admin/[controller]")]
    [ApiController, ApiKeyRequired, Authorize(Roles = ApplicationRoles.ADMIN)]
    public class DashboardController : ControllerBase
    {
        private readonly ExceptionHandler<DashboardController> _exceptionHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardController"/> class.
        /// </summary>
        /// <param name="exceptionHandler">The exception handler specific to the <see cref="DashboardController"/>.</param>
        public DashboardController(ExceptionHandler<DashboardController> exceptionHandler)
        {
            _exceptionHandler = exceptionHandler;
        }

        /// <summary>
        /// Get current stat.
        /// </summary>
        /// <returns>
        /// Returns current stat of total products, total categories, 
        /// total orders, total paid amount, total returned amount and many more
        /// </returns>
        [HttpGet("current-stat")]
        public async Task<IActionResult> CurrentStat()
        {
            await Task.CompletedTask;
            return Ok();
        }
    }
}
