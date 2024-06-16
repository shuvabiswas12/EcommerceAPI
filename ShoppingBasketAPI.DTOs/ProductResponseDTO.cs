using ShoppingBasketAPI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasketAPI.DTOs
{
    public class ProductResponseDTO
    {
        public IEnumerable<Product> products { get; set; } = null!;
        public int totalProducts { get; set; } = 0;
    }
}
