using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingBasketAPI.Services.IServices;
using ShoppingBasketAPI.Utilities.Exceptions.Handler;

namespace ShoppingBasketAPI.Api.Controllers
{
    /// <summary>
    /// This controller is responsible for managing current availability of any products.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class QuantityController : ControllerBase
    {
        private readonly IQuantityServices _quantityServices;
        private readonly ExceptionHandler<QuantityController> exceptionHandler;

        public QuantityController(IQuantityServices quantityServices, ExceptionHandler<QuantityController> exceptionHandler)
        {
            _quantityServices = quantityServices;
            this.exceptionHandler = exceptionHandler;
        }
    }
}
