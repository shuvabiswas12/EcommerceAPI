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

            public class UserInfo
            {
                public required string Id { get; set; }
                public required string Email { get; set; }
            }
        }
    }
}

