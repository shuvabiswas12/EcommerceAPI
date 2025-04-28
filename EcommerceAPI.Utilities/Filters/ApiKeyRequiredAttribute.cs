using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAPI.Utilities.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiKeyRequiredAttribute : Attribute, IAsyncActionFilter
    {
        private const string _apiKeyInHeader = "x-api-key";
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(_apiKeyInHeader, out var potentialKey))
            {
                context.Result = new UnauthorizedObjectResult(new { Error = ResponseMessages.StatusCode_401_Unauthorized, Message = "API key is missing" });
                return;
            }

            var config = context.HttpContext.RequestServices.GetService<IConfiguration>() ?? throw new Exception("Configuration json settings was not found.");
            var defaultApiKey = config.GetSection(key: "ApiKey").Value ?? throw new Exception("ApiKey was not found in the app's json settings.");
            if (!defaultApiKey.Equals(potentialKey))
            {
                context.Result = new UnauthorizedObjectResult(new { Error = ResponseMessages.StatusCode_401_Unauthorized, Message = "Invalid API key" });
                return;
            }

            await next();
        }
    }
}
