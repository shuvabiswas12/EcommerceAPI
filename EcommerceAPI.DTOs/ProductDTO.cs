using Microsoft.EntityFrameworkCore;
using EcommerceAPI.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public class DiscountDTO
    {
        public double DiscountRate { get; set; }
        public Boolean DiscountEnabled { get; set; }
        public DateTime? DiscountStartAt { get; set; }
        public DateTime? DiscountEndAt { get; set; }
    }

    public class CategoryDTO
    {
        public string Id { get; set; } = null!;
        public string Name { set; get; } = null!;
    }
}
