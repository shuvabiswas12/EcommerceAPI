using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EcommerceAPI.DTOs;
using EcommerceAPI.Services.IServices;
using EcommerceAPI.Utilities;
using EcommerceAPI.Utilities.Exceptions;
using EcommerceAPI.Utilities.Filters;
using EcommerceAPI.Utilities.Validation;

namespace EcommerceAPI.Api.Controllers.Auth
{
    /// <summary>
    /// Controller for authentication operations such as login and signup in the Shopping Basket API.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationServices _authenticationServices;

        /// <summary>
        /// Constructor for AuthController.
        /// </summary>
        public AuthController(IAuthenticationServices authenticationServices)
        {
            _authenticationServices = authenticationServices;
        }

        /// <summary>
        /// Endpoint for user login.
        /// </summary>
        /// <param name="loginRequestDTO">DTO containing login credentials.</param>
        [HttpPost("Login"), ApiKeyRequired]
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
        [HttpPost("Signup"), ApiKeyRequired]
        public async Task<IActionResult> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            var modelState = ModelValidator.ValidateModel(registrationRequestDTO);

            if (!modelState.IsValid)
            {
                var errors = ModelValidator.GetErrors(modelState);
                return BadRequest(new { Error = errors });
            }
            var createdUser = await _authenticationServices.Register(registrationRequestDTO);
            return Ok(createdUser);
        }
    }
}
