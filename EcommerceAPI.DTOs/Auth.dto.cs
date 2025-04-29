using EcommerceAPI.Utilities.Validation.CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAPI.DTOs
{
    public class RegistrationRequestDTO
    {
        [NotEmpty(errorMessage: "FirstName is required.")] public string FirstName { get; set; } = null!;
        [NotEmpty(errorMessage: "LastName is required.")] public string LastName { get; set; } = null!;

        [NotEmpty(errorMessage: "Email is required."),
            ValidEmailAddress(errorMessage: "Invalid email address. Please provide a Gmail, Yahoo, or Hotmail email.")]
        public string Email { get; set; } = null!;

        [NotEmpty(errorMessage: "Password is required."),
            ValidPassword(errorMessage: "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special symbol (@, #, $)")]
        public string Password { get; set; } = null!;
    }

    public class RegistrationResponseDTO
    {
        public required Boolean Success { get; set; }
        public required UserInfo User { get; set; }

        public class UserInfo
        {
            public required string Id { get; set; }
        }
    }

    public class LoginRequestDTO
    {
        [NotEmpty(errorMessage: "Email is required.")] public string Email { get; set; } = string.Empty;

        [NotEmpty(errorMessage: "Password is required."),] public string Password { get; set; } = string.Empty;
    }

    public class LoginResponseDTO
    {
        public required Boolean Success { get; set; }
        public required String AccessToken { get; set; }
    }

    public class GoogleSignInPayload
    {
        [Required] required public string IdToken { get; set; } = null!;
    }

    public class GoogleAuthConfigDTO
    {
        public string ClientId { get; set; } = null!;
        public string ClientSecret { get; set; } = null!;
    }
}
