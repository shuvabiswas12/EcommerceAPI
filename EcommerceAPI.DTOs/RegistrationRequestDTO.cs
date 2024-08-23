using EcommerceAPI.Utilities.Validation.CustomAttributes;
using System;
using System.Collections.Generic;
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
}
