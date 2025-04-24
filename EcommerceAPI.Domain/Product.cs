using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EcommerceAPI.Domain
{
    public class Product
    {
        [Key] public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required] public required string Name { get; set; } = null!;
        [Required] public required string Description { get; set; } = null!;
        [Required, Precision(10, 2)] public required decimal Price { get; set; }
        public ICollection<Image> Images { get; set; } = new List<Image>();
        public Boolean IsFeatured { get; set; } = false;
        [JsonIgnore] public string CategoryId { get; set; } = null!;
        [ForeignKey("CategoryId"), ValidateNever] public Category Category { get; set; } = null!;
        public ProductAvailability ProductAvailability { get; set; } = null!;
        public Discount Discount { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}