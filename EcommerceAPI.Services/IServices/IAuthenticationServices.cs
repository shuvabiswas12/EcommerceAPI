using EcommerceAPI.Domain;
using EcommerceAPI.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAPI.Services.IServices
{
    public interface IAuthenticationServices
    {
        public Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        public Task<RegistrationResponseDTO> Register(RegistrationRequestDTO registrationRequestDTO);
        public Task<ApplicationUser?> GetUserByIdAsync(string applicationUserId);

        /// <summary>
        /// Generate JWT Token
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Return a string type token.</returns>
        public Task<string> GenerateJwtToken(ApplicationUser user);
    }
}
