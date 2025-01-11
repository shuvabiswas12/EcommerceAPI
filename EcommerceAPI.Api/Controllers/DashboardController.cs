using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using EcommerceAPI.Utilities.ApplicationRoles;
using EcommerceAPI.Utilities.Filters;
using Asp.Versioning;

namespace EcommerceAPI.Api.Controllers.Admin
{
    /// <summary>
    /// Controller for managing dashboard-related operations.
    /// </summary>
    [ApiVersion(2.0), Route("api/admin/v{version:apiVersion}/[controller]")]
    [ApiController, ApiKeyRequired, Authorize(Roles = ApplicationRoles.ADMIN)]
    public class DashboardController : ControllerBase
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardController"/> class.
        /// </summary>
        public DashboardController()
        {

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
