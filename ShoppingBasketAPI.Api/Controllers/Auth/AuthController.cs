using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingBasketAPI.DTOs;
using ShoppingBasketAPI.Services.IServices;
using ShoppingBasketAPI.Utilities;
using ShoppingBasketAPI.Utilities.Exceptions;
using ShoppingBasketAPI.Utilities.Validation;

namespace ShoppingBasketAPI.Api.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationServices _authenticationServices;
        private ILogger<AuthController> _logger;

        public AuthController(IAuthenticationServices authenticationServices, ILogger<AuthController> logger)
        {
            _authenticationServices = authenticationServices;
            _logger = logger;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequestDTO loginRequestDTO)
        {
            var modelState = ModelValidator.ValidateModel(loginRequestDTO);
            if (!modelState.IsValid)
            {
                var errors = modelState.Where(ms => ms.Value!.Errors.Any())
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value!.Errors.Select(err => err.ErrorMessage))
                    .ToArray();
                return BadRequest(new { Error = errors });
            }
            try
            {
                var response = await _authenticationServices.Login(loginRequestDTO);
                return Ok(response);
            }
            catch (InvalidLoginException ex)
            {
                _logger.LogError(ex, "\n An error occured while creating new product. \t\n" + ex.Message);
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "\n An error occured while creating new product. \t\n" + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { Error = ResponseMessages.StatusCode_500_ErrorMessage });
            }
        }

        [HttpPost("Signup")]
        public async Task<IActionResult> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            var modelState = ModelValidator.ValidateModel(registrationRequestDTO);

            if (!modelState.IsValid)
            {
                var errors = modelState.Where(ms => ms.Value!.Errors.Any())
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value!.Errors.Select(err => err.ErrorMessage))
                    .ToArray();
                return BadRequest(new { Error = errors });
            }
            try
            {
                var createdUser = await _authenticationServices.Register(registrationRequestDTO);
                return Ok(createdUser);
            }
            catch (DuplicateEntriesFoundException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "\n An error occured while creating new product. \t\n" + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { Error = ResponseMessages.StatusCode_500_ErrorMessage });
            }
        }
    }
}
