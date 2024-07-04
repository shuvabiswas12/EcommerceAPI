using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ShoppingBasketAPI.DTOs
{
    public class CartCreateDTO
    {
        [ValidateNever, JsonIgnore]
        public string UserId { get; set; } = String.Empty;

        [ValidateNever, JsonIgnore]
        public string ProductId { get; set; } = String.Empty;

        public int Count { get; set; } = 1;
    }
}
