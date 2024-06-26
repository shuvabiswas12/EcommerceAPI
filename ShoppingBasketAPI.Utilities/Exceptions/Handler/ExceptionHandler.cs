using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasketAPI.Utilities.Exceptions.Handler
{
    /// <summary>
    /// This is a Exception Handler. The task of this class is handle the exception and log the errors.
    /// </summary>
    /// <typeparam name="T">T means Controller name or class name where it will be injected by DI.</typeparam>
    public class ExceptionHandler<T> where T : class
    {
        private readonly ILogger<T> _logger;

        public ExceptionHandler(ILogger<T> logger)
        {
            _logger = logger;
        }

        public IActionResult HandleException(Exception ex, string message)
        {
            _logger.LogError(ex, "\n =>=>=>" + message + "\n\n");
            return new ObjectResult(new { Error = ResponseMessages.StatusCode_500_ErrorMessage })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }
    }
}