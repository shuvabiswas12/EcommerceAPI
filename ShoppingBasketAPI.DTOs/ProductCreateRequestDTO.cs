using ShoppingBasketAPI.Utilities.Validation.CustomAttributes;
using ShoppingBasketAPI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasketAPI.DTOs
{
    public class ProductCreateRequestDTO
    {
        [NotEmpty(errorMessage: "Name field is required.")] public string Name { get; set; } = null!;
        [NotEmpty(errorMessage: "Description field is required.")] public string Description { get; set; } = null!;
        [GreaterThan(0, "Price field must be greater than zero.")] public decimal Price { get; set; }
        [MinimumOneImageUrl("At least one image URL is required.")] public ICollection<string> ImageUrls { get; set; } = new List<string>();
    }
}
