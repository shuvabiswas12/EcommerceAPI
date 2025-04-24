using EcommerceAPI.Utilities.Validation.CustomAttributes;
using EcommerceAPI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAPI.DTOs
{
    public class ProductCreateDTO
    {
        [NotEmpty("Category must be required.")] public string CategoryId { get; set; } = null!;
        [NotEmpty(errorMessage: "Name field is required.")] public string Name { get; set; } = null!;
        [NotEmpty(errorMessage: "Description field is required.")] public string Description { get; set; } = null!;
        [GreaterThan(0, "Price field must be greater than zero.")] public decimal Price { get; set; }
        [MinimumOneImageUrl("At least one image URL is required.")] public ICollection<string> ImageUrls { get; set; } = new List<string>();
    }
}
