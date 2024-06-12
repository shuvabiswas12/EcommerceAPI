using ShoppingBasketAPI.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasketAPI.DTOs
{
    public class CategoryResponseDTO
    {
        public IEnumerable<Category> Categories { get; set; } = null!;
        public int TotalCategories { get; set; } = 0;
    }
}
