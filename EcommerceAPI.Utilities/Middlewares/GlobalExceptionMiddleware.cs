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
            context.Response.ContentType = "application/json";

            HttpStatusCode statusCode;
            object? errors = null;

            switch (exception)
            {
                case ApiException ex:
                    statusCode = ex.StatusCode;
                    errors = ex.ErrorDetails;
                    break;

                case UnauthorizedAccessException ex:
                    statusCode = HttpStatusCode.Unauthorized;
                    errors = string.IsNullOrWhiteSpace(ex.Message) ? null : new { error = ex.Message };
                    break;

                case ModelValidationException ex:
                    statusCode = HttpStatusCode.BadRequest;
                    errors = string.IsNullOrWhiteSpace(ex.Message) ? null : new { error = ex.ValidationErrors };
                    break;

                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    errors = new { error = "An unexpected error occurred." };
                    break;
            }
            context.Response.StatusCode = (int)statusCode;
            if (errors is null) return Task.CompletedTask;

            var result = JsonSerializer.Serialize(errors);
            return context.Response.WriteAsync(result);
        }
    }
}