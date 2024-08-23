using EcommerceAPI.Utilities.Validation.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAPI.DTOs
{
    public class FeaturedProductRequestDTO
    {
        [NotEmpty(errorMessage: "Product id is required")]
        public string Id { get; set; } = null!;
    }
}
