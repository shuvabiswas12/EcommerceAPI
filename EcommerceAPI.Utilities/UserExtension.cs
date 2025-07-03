using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAPI.Utilities
{
    public static class UserExtension
    {
        /// <summary>
        /// Get User id of any authenticated user.
        /// </summary>
        /// <param name="user">Takes one parameter as user which is a object of ClaimsPrinciple.</param>
        /// <returns>It Returns authenticated User's Id. It not authenticated it returns empty string.</returns>
        public static string GetUserId(this ClaimsPrincipal user)
        {
            if (user?.Identity?.IsAuthenticated != true)
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            var userId = (user.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.Actor)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                throw new InvalidOperationException("User ID is missing or invalid.");
            }

            return userId;
        }
    }
}
