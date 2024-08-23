using System;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using System.Net;
using System.Text.Json;

namespace EcommerceAPI.Utilities.Middlewares
{
    /// <summary>
    /// This is a Global Exception Handler Middleware and it will work throughout the application by adding this in DI conatiner.
    /// </summary>
    public class ExceptionHandleMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionHandleMiddleware> _logger;

        public ExceptionHandleMiddleware(ILogger<ExceptionHandleMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"\n Something went wrong.\n {ex.Message} \n");
                await HandleException(context, ex);
            }
        }

        public static async Task HandleException(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            int statusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.StatusCode = statusCode;
            var errorResponse = new GlobalErrorResponse
            {
                Error = ResponseMessages.StatusCode_500_ErrorMessage,
                StatusCode = statusCode
            };
            await context.Response.WriteAsync(errorResponse.ToString());
        }
    }
}