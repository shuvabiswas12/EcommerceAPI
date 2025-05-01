using Microsoft.EntityFrameworkCore;
using EcommerceAPI.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcommerceAPI.Utilities.Validation.CustomAttributes;

namespace EcommerceAPI.DTOs
{
    public class ProductDTO
    {
        public required string Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int CurrentAvailability { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsFeatured { get; set; }
        public DiscountDTO Discount { get; set; } = null!;
        public double DiscountPrice { get; set; }
        public CategoryDTO Category { get; set; } = null!;
        public ICollection<string> Images { get; set; } = new List<string>();
    }

    public class ProductCreateDTO
    {
        [NotEmpty("Category must be required.")] public string CategoryId { get; set; } = null!;
        [NotEmpty(errorMessage: "Name field is required.")] public string Name { get; set; } = null!;
        [NotEmpty(errorMessage: "Description field is required.")] public string Description { get; set; } = null!;
        [GreaterThan(0, "Price field must be greater than zero.")] public decimal Price { get; set; }
        public int? CurrentAvailability { get; set; } = 0;
        public bool? IsFeatured { get; set; }
        public double? DiscountRate { get; set; }
        [MinimumOneImageUrl("At least one image URL is required.")] public ICollection<string> ImageUrls { get; set; } = new List<string>();
    }

    public class ProductQuantityDTO
    {
        public int Quantity { get; set; } = 0;
    }

    public class ProductUpdateDTO
    {
        public string? Name { get; set; } = null;
        public string? Description { get; set; } = null;
        public decimal? Price { get; set; } = 0m;
        public string? CategoryId { get; set; }
        public int? CurrentAvailability { get; set; }
        public int? IsFeatured { get; set; }
        public ICollection<string>? ImageUrls { get; set; } = new List<string>();
        public double DiscountRate { get; set; } = 0.0;
        public int? DiscountEnabled { get; set; }
        public long? DiscountStartTimestamp { get; set; } = null;
        public long? DiscountEndTimestamp { get; set; } = null;
    }

    public class FeaturedProductRequestDTO
    {
        [NotEmpty(errorMessage: "Product id is required")] public string Id { get; set; } = null!;
    }
}
