using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EcommerceAPI.DTOs;
using EcommerceAPI.Services.IServices;
using EcommerceAPI.Utilities;
using EcommerceAPI.Utilities.Exceptions;
using EcommerceAPI.Utilities.Filters;
using EcommerceAPI.Utilities.Validation;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using EcommerceAPI.Domain;

namespace EcommerceAPI.Api.Controllers.Auth
{
    /// <summary>
    /// Controller for authentication operations such as login and signup in the Shopping Basket API.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController, ApiKeyRequired]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationServices _authenticationServices;
        private readonly IGoogleAuthService _googleAuthService;

        /// <summary>
        /// Constructor for AuthController.
        /// </summary>
        public AuthController(IAuthenticationServices authenticationServices, IGoogleAuthService googleAuthService)
        {
            _authenticationServices = authenticationServices;
            _googleAuthService = googleAuthService;
        }

        /// <summary>
        /// Endpoint for user login.
        /// </summary>
        /// <param name="loginRequestDTO">DTO containing login credentials.</param>
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequestDTO loginRequestDTO)
        {
            var modelState = ModelValidator.ValidateModel(loginRequestDTO);
            if (!modelState.IsValid)
            {
                var errors = ModelValidator.GetErrors(modelState);
                return BadRequest(new { Error = errors });
            }
            var response = await _authenticationServices.Login(loginRequestDTO);
            return Ok(response);
        }

        /// <summary>
        /// Endpoint for user registration.
        /// </summary>
        /// <param name="registrationRequestDTO">DTO containing registration details.</param>
        [HttpPost("Signup")]
        public async Task<IActionResult> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            var modelState = ModelValidator.ValidateModel(registrationRequestDTO);

            if (!modelState.IsValid)
            {
                var errors = ModelValidator.GetErrors(modelState);
                return BadRequest(new { Error = errors });
            }
            var createdUser = await _authenticationServices.Register(registrationRequestDTO);
            return StatusCode(StatusCodes.Status201Created, createdUser);
        }

        /// <summary>
        /// Google login
        /// </summary>
        /// <param name="idToken">Google's Id Token</param>
        /// <returns>A token</returns>
        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleSignInPayload payload)
        {
            if (payload.IdToken == null)
            {
                return Unauthorized("Invalid Google Token");
            }
            var token = await _googleAuthService.GoogleSignIn(payload);
            if (token == null) return Unauthorized("Google authentication failed.");
            return Ok(new { token = token });
        }
    }
}
