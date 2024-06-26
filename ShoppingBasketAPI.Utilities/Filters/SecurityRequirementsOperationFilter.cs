using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasketAPI.Utilities.Filters
{
    public class SecurityRequirementsOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var apiKeyRequired = context.MethodInfo.DeclaringType!.GetCustomAttributes(true)
                                             .Union(context.MethodInfo.GetCustomAttributes(true))
                                             .OfType<ApiKeyRequiredAttribute>().Any();

            var authorizeAttributes = context.MethodInfo.DeclaringType!.GetCustomAttributes(true)
                                     .Union(context.MethodInfo.GetCustomAttributes(true))
                                     .OfType<AuthorizeAttribute>();

            if (apiKeyRequired)
            {
                operation.Security.Add(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "ApiKey"
                        }
                    },
                    new string[] {}
                }
            });
            }

            if (authorizeAttributes.Any())
            {
                operation.Security.Add(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    authorizeAttributes.SelectMany(attr => attr.Roles?.Split(',') ?? new string[] {}).ToArray()
                }
            });
            }
        }
    }
}
