using ShoppingBasketAPI.Utilities.Validation.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasketAPI.DTOs
{
    public class LoginResponseDTO
    {
        public required string Status { get; set; }

        public class Data
        {
            public required string AccessToken { get; set; }
            public required LoginUserInfoResponseDTO User { get; set; }
        }
    }

    public class LoginUserInfoResponseDTO
    {
        public required string Id { get; set; }
        public required string Email { get; set; }

        public LoginUserInfoResponseDTO(string id, string email)
        {
            Id = id;
            Email = email;
        }
    }
}

