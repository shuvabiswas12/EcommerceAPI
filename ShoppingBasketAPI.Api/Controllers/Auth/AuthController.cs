using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingBasketAPI.DTOs;
using ShoppingBasketAPI.Services.IServices;
using ShoppingBasketAPI.Utilities;
using ShoppingBasketAPI.Utilities.Exceptions;
using ShoppingBasketAPI.Utilities.Exceptions.Handler;
using ShoppingBasketAPI.Utilities.Validation;

namespace ShoppingBasketAPI.Api.Controllers.Auth
{
    /// <summary>
    /// Controller for authentication operations such as login and signup in the Shopping Basket API.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationServices _authenticationServices;
        private readonly ExceptionHandler<AuthController> _exceptionHandler;

        /// <summary>
        /// Constructor for AuthController.
        /// </summary>
        /// <param name="authenticationServices">The service handling authentication operations.</param>
        /// <param name="exceptionHandler">Exception handler for handling controller-level exceptions.</param>
        public AuthController(IAuthenticationServices authenticationServices, ExceptionHandler<AuthController> exceptionHandler)
        {
            _authenticationServices = authenticationServices;
            _exceptionHandler = exceptionHandler;
        }

        /// <summary>
        /// Endpoint for user login.
        /// </summary>
        /// <param name="loginRequestDTO">DTO containing login credentials.</param>
        /// <returns>Returns an IActionResult representing the login operation result.</returns>
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequestDTO loginRequestDTO)
        {
            var modelState = ModelValidator.ValidateModel(loginRequestDTO);
            if (!modelState.IsValid)
            {
                var errors = ModelValidator.GetErrors(modelState);
                return BadRequest(new { Error = errors });
            }
            try
            {
                var response = await _authenticationServices.Login(loginRequestDTO);
                return Ok(response);
            }
            catch (InvalidLoginException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return _exceptionHandler.HandleException(ex, "An error occured while creating new product.");
            }
        }

        /// <summary>
        /// Endpoint for user registration.
        /// </summary>
        /// <param name="registrationRequestDTO">DTO containing registration details.</param>
        /// <returns>Returns an IActionResult representing the registration operation result.</returns>
        [HttpPost("Signup")]
        public async Task<IActionResult> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            var modelState = ModelValidator.ValidateModel(registrationRequestDTO);

            if (!modelState.IsValid)
            {
                var errors = ModelValidator.GetErrors(modelState);
                return BadRequest(new { Error = errors });
            }
            try
            {
                var createdUser = await _authenticationServices.Register(registrationRequestDTO);
                return Ok(createdUser);
            }
            catch (DuplicateEntriesFoundException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return _exceptionHandler.HandleException(ex, "An error occured while creating new product.");
            }
        }
    }
}
