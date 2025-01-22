using EcommerceAPI.Data.UnitOfWork;
using EcommerceAPI.Domain;
using EcommerceAPI.DTOs;
using EcommerceAPI.Services.IServices;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAPI.Services.Services
{
    public class GoogleAuthService : IGoogleAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly GoogleAuthConfigDTO _googleAuthConfig;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthenticationServices _authenticationServices;

        public GoogleAuthService(IUnitOfWork unitOfWork, IOptions<GoogleAuthConfigDTO> googleAuthConfig, UserManager<ApplicationUser> userManager, IAuthenticationServices authenticationServices)
        {
            _unitOfWork = unitOfWork;
            _googleAuthConfig = googleAuthConfig.Value;
            _userManager = userManager;
            _authenticationServices = authenticationServices;
        }

        private async Task<GoogleJsonWebSignature.Payload?> ValidateGoogleTokenAsync(string idToken)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { _googleAuthConfig.ClientId }
            };
            return await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
        }

        public async Task<string> GoogleSignIn(GoogleSignInPayload model)
        {
            var payload = await ValidateGoogleTokenAsync(model.IdToken);
            if (payload == null)
            {
                return string.Empty;
            }

            var email = payload.Email;
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new ApplicationUser { Email = email, UserName = email };
                await _userManager.CreateAsync(user);
            }
            // Generate JWT
            var token = await _authenticationServices.GenerateJwtToken(user);
            return token;
        }
    }
}
