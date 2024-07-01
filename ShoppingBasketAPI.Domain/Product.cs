using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ShoppingBasketAPI.Domain
{
    public class Product
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required] public string Name { get; set; } = null!;
        [Required] public string Description { get; set; } = null!;
        [Required, Precision(10, 2)] public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Computed property.
        [NotMapped]
        public bool IsFeatured => FeaturedProduct != null;

        // Computed property.
        [NotMapped]
        public double DiscountRate => Discount != null ? Discount.DiscountRate : 0.0;

        // One to many relationship
        // Navigation property
        public ICollection<Image> Images { get; set; } = new List<Image>();

        // Navigation properties
        [JsonIgnore] public Discount Discount { get; set; } = null!;
        [JsonIgnore] public FeaturedProduct FeaturedProduct { get; set; } = null!;
        [JsonIgnore] public ProductCategory ProductCategory { get; set; } = null!;
    }
}