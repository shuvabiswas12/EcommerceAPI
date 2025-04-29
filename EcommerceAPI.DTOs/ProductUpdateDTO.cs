using EcommerceAPI.Utilities.Validation.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAPI.DTOs
{
    public class ProductUpdateDTO
    {
        public string? Name { get; set; } = null;
        public string? Description { get; set; } = null;
        public decimal? Price { get; set; } = 0m;
        public string? CategoryId { get; set; }
        public ICollection<string>? ImageUrls { get; set; } = new List<string>();
    }
}
