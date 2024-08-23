using EcommerceAPI.Utilities.Validation.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAPI.DTOs
{
    public class LoginResponseDTO
    {
        public required string Status { get; set; }
        public required UserData Data { get; set; }

        public class UserData
        {
            public required string AccessToken { get; set; }
            public required UserInfo User { get; set; }
        }

        public class UserInfo
        {
            public required string Id { get; set; }
            public required string Email { get; set; }
        }
    }
}