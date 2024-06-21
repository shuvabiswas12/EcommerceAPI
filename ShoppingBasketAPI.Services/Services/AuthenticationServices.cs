using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ShoppingBasketAPI.Domain;
using ShoppingBasketAPI.DTOs;
using ShoppingBasketAPI.Services.IServices;
using ShoppingBasketAPI.Utilities;
using ShoppingBasketAPI.Utilities.ApplicationRoles;
using ShoppingBasketAPI.Utilities.Exceptions;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasketAPI.Services.Services
{
    public class AuthenticationServices : IAuthenticationServices
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthenticationServices(RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
            _roleManager = roleManager;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginRequestDTO.Email);
            if (user == null)
            {
                throw new InvalidLoginException(message: "Invalid email or password.");
            }
            var isPasswordMatched = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);
            if (!isPasswordMatched)
            {
                throw new InvalidLoginException(message: "Invalid email or password");
            }
            var token = await GenerateJwtToken(user);
            var loginResponse = new LoginResponseDTO
            {
                Status = "Success",
                Data = new LoginResponseDTO.UserData
                {
                    AccessToken = token,
                    User = new LoginResponseDTO.UserInfo
                    {
                        Email = loginRequestDTO.Email,
                        Id = user.Id
                    }
                }
            };
            return loginResponse;
        }

        private async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email!),
                new Claim(ClaimTypes.Actor, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            // getting roles of user
            var userRoles = await _userManager.GetRolesAsync(user);
            if (userRoles.Any())
            {
                // adding roles to authClaims
                foreach (var role in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            var authSignInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"] ?? throw new ArgumentNullException("JWT secret key was not found.")));

            var tokenProperty = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"] ?? throw new ArgumentNullException("JWT valid issuer was not found."),
                audience: _configuration["JWT:ValidAudience"] ?? throw new ArgumentNullException("JWT ValidAudience was not found."),
                expires: DateTime.Now.AddHours(24),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSignInKey, SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(tokenProperty);
        }

        public async Task<RegistrationResponseDTO> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            var user = await _userManager.FindByEmailAsync(registrationRequestDTO.Email);
            if (user is not null)
            {
                throw new DuplicateEmailException("This email has taken already.");
            }
            ApplicationUser newUser = new ApplicationUser
            {
                Email = registrationRequestDTO.Email,
                UserName = registrationRequestDTO.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var result = await _userManager.CreateAsync(newUser, registrationRequestDTO.Password);

            // If there are any errors occured
            if (!result.Succeeded)
            {
                var msg = "";
                foreach (var error in result.Errors)
                {
                    msg += error.Description;
                }
                throw new Exception(message: msg);
            }

            // Creating roles if not exists.
            await CreateRoles();

            // Add new user to a role.
            await _userManager.AddToRoleAsync(newUser, IApplicationRoles.WEB_USER);
            return new RegistrationResponseDTO
            {
                IsSuccess = true,
                Message = "Your account has been created successfully.",
                User = new RegistrationResponseDTO.UserInfo
                {
                    Id = newUser.Id
                }
            };
        }

        private async Task CreateRoles()
        {
            if (!await _roleManager.RoleExistsAsync(IApplicationRoles.ADMIN))
            {
                await _roleManager.CreateAsync(new IdentityRole(IApplicationRoles.ADMIN));
            }
            if (!await _roleManager.RoleExistsAsync(IApplicationRoles.EMPLOYEE))
            {
                await _roleManager.CreateAsync(new IdentityRole(IApplicationRoles.EMPLOYEE));
            }
            if (!await _roleManager.RoleExistsAsync(IApplicationRoles.WEB_USER))
            {
                await _roleManager.CreateAsync(new IdentityRole(IApplicationRoles.WEB_USER));
            }
            await Task.CompletedTask;
        }
    }
}
