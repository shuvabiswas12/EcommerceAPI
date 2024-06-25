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

namespace ShoppingBasketAPI.Utilities.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiKeyRequiredAttribute : Attribute, IAsyncActionFilter
    {
        private const string _apiKeyInHeader = "ApiKey";
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(_apiKeyInHeader, out var potentialKey))
            {
                context.Result = new UnauthorizedObjectResult(new { Error = ResponseMessages.StatusCode_401_Unauthorized, Message = "API key is missing" });
                return;
            }

            var config = context.HttpContext.RequestServices.GetService<IConfiguration>();
            var defaultApiKey = config.GetSection(key: "ApiKey").Value;
            if (!defaultApiKey.Equals(potentialKey))
            {
                context.Result = new UnauthorizedObjectResult(new { Error = ResponseMessages.StatusCode_401_Unauthorized, Message = "Invalid API key" });
                return;
            }

            await next();
        }
    }
}
