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
using EcommerceAPI.Utilities.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace EcommerceAPI.Utilities.Middlewares
{
    /// <summary>
    /// This is a Global Exception Handler Middleware and it will work throughout the application.
    /// </summary>
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred: {ex.Message}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            HttpStatusCode statusCode = HttpStatusCode.InternalServerError; // Default to 500 Internal Server Error
            string result;

            switch (exception)
            {
                case NotFoundException ex:
                    statusCode = HttpStatusCode.NotFound;
                    result = JsonSerializer.Serialize(new { error = ex.Message });
                    break;

                case ValidationException ex:
                    statusCode = HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(new { error = ex.Message });
                    break;

                case UnauthorizedAccessException ex:
                    statusCode = HttpStatusCode.Unauthorized;
                    result = JsonSerializer.Serialize(new { error = ex.Message });
                    break;

                case InvalidOperationException ex:
                    statusCode = HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(new { error = ex.Message });
                    break;

                case DuplicateEntriesException ex:
                    statusCode = HttpStatusCode.Conflict;
                    result = JsonSerializer.Serialize(new { error = ex.Message });
                    break;

                case ArgumentNullException ex:
                    statusCode = HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(new { error = ex.Message });
                    break;

                case ArgumentOutOfRangeException ex:
                    statusCode = HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(new { error = ex.Message });
                    break;

                case ArgumentException ex:
                    statusCode = HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(new { error = ex.Message });
                    break;

                default:
                    result = JsonSerializer.Serialize(new { error = "An unexpected error occurred." });
                    break;
            }
            context.Response.StatusCode = (int)statusCode;
            return context.Response.WriteAsync(result);
        }
    }
}