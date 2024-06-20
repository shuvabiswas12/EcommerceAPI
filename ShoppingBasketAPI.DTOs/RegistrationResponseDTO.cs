using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasketAPI.DTOs
{
    public class RegistrationResponseDTO
    {
        public required bool IsSuccess { get; set; }
        public required string Message { get; set; }
        public required RegistrationUserInfoResponseDTO User { get; set; }
    }

    public class RegistrationUserInfoResponseDTO
    {
        public required string Id { get; set; }
        public RegistrationUserInfoResponseDTO(string id)
        {
            Id = id;
        }
    }
}
