using EcommerceAPI.Utilities.Validation.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAPI.DTOs
{
    public class LoginRequestDTO
    {
        [NotEmpty(errorMessage: "Email is required.")]
        public string Email { get; set; } = string.Empty;

        [NotEmpty(errorMessage: "Password is required."),]
        public string Password { get; set; } = string.Empty;
    }
}
