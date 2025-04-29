using EcommerceAPI.Domain;
using EcommerceAPI.Utilities.Validation.CustomAttributes;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EcommerceAPI.DTOs
{
    public class CartCreateDTO
    {
        [JsonIgnore] public required string UserId { get; set; }
        [NotEmpty(errorMessage: "Product id is required")] public string ProductId { get; set; } = String.Empty;
        public int Count { get; set; } = 1;
    }

    public class CartResponseDTO
    {
        public IEnumerable<ShoppingCart>? ShoppingCarts { get; set; }
        public double TotalCost { get; set; } = 0.0;
    }
}
