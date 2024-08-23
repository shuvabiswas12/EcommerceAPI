using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAPI.DTOs
{
    public class RegistrationResponseDTO
    {
        public required bool IsSuccess { get; set; }
        public required string Message { get; set; }
        public required UserInfo User { get; set; }

        public class UserInfo
        {
            public required string Id { get; set; }
        }
    }
}
