using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingBasketAPI.DTOs;

namespace ShoppingBasketAPI.Api.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequestDTO loginRequestDTO)
        {
            return Ok();
        }

        [HttpPost("Signup")]
        public async Task<IActionResult> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            return Ok();
        }
    }
}
