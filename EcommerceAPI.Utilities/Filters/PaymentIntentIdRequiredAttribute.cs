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
    [AttributeUsage(AttributeTargets.Method)]
    public class PaymentIntentIdRequiredAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.HttpContext.Request.Headers.TryGetValue(HeaderKeys.PaymentIntentId, out var _))
            {
                context.Result = new UnauthorizedObjectResult(new { Error = ResponseMessages.StatusCode_401_Unauthorized, Message = "Payment intent id is missing" });
                return;
            }
            await next();
        }
    }
}
