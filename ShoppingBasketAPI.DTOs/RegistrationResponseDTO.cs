using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasketAPI.DTOs
{
    public class RegistrationResponseDTO
    {
        public required string Message { get; set; }
        public required bool IsSuccess { get; set; }
        public required User User { get; set; }
    }
    public class User
    {
        public required string Id { get; set; }
    }
}
