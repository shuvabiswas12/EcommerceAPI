using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingBasketAPI.Utilities.Exceptions.Handler;

namespace ShoppingBasketAPI.Api.Controllers.Admin
{
    /// <summary>
    /// Controller for managing dashboard-related operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
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
    }
}
