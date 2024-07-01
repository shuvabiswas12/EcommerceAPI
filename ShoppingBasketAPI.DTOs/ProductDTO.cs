using Microsoft.EntityFrameworkCore;
using ShoppingBasketAPI.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasketAPI.DTOs
{
    public class ProductDTO
    {
        public required string Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsFeatured { get; set; }
        public double DiscountRate { get; set; }
        public double DiscountPrice { get; set; } = 0.0;
        public ICollection<ImageResponseDTO> Images { get; set; } = new List<ImageResponseDTO>();
    }
}
