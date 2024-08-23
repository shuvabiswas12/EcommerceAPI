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
            if (user.Identity?.IsAuthenticated == true)
            {
                return (user.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.Actor)?.Value!;
            }
            return string.Empty;
        }
    }
}
