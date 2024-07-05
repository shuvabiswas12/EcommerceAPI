using ShoppingBasketAPI.Domain;
using ShoppingBasketAPI.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasketAPI.Services.IServices
{
    public interface IAuthenticationServices
    {
        public Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        public Task<RegistrationResponseDTO> Register(RegistrationRequestDTO registrationRequestDTO);
        public Task<ApplicationUser?> GetUserByIdAsync(string applicationUserId);
    }
}
