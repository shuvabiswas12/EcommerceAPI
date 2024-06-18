using ShoppingBasketAPI.Utilities.Validation.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasketAPI.DTOs
{
    public class ProductUpdateRequestDTO
    {
        public string? Name { get; set; } = null;
        public string? Description { get; set; } = null;
        public decimal? Price { get; set; } = 0m;
        public ICollection<string>? ImageUrls { get; set; } = new List<string>();
    }
}
