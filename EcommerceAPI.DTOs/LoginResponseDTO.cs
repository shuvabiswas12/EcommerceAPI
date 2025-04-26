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
        public required Boolean Success { get; set; }
        public required String AccessToken { get; set; }

    }
}