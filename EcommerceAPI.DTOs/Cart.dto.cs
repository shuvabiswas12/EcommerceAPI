using EcommerceAPI.Domain;
using EcommerceAPI.Utilities;
using EcommerceAPI.Utilities.Validation.CustomAttributes;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EcommerceAPI.DTOs
{
    public class CartCreateDTO
    {
        [JsonIgnore] public string UserId { get; set; } = default!;
        [NotEmpty(errorMessage: "Product id is required")] public string ProductId { get; set; } = String.Empty;
        public int Count { get; set; } = 1;
    }

    public class CartResponseDTO
    {
        public IEnumerable<CartDTO>? Carts { get; set; }
        public decimal TotalCost { get; set; }
        public string? UserId { get; set; } = default;
    }

    public class CartDTO
    {
        public int Count { get; set; }
        [JsonIgnore] public DateTime CreatedAt { get; set; }
        public long CreatedTimestamp => Timestamps.GetTimestamp(CreatedAt);
        public string ProductName { get; set; }
        public string ProductId { get; set; }
        public List<string> ImageUrls { get; set; } = new List<string>();
        public decimal Price { get; set; }
    }
}
