using EcommerceAPI.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAPI.Services.IServices
{
    public interface IGoogleAuthService
    {
        /// <summary>
        /// Method to sign in a user using Google.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>It returns a string based token.</returns>
        Task<string> GoogleSignIn(GoogleSignInPayload model);
    }
}
