using EcommerceAPI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAPI.DTOs
{
    public class CartResponseDTO
    {
        public IEnumerable<ShoppingCart>? ShoppingCarts { get; set; }
        public double TotalCost { get; set; } = 0.0;
    }
}
