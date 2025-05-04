using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAPI.DTOs
{
    public class WishlistDTO
    {
        public string User { get; set; }
        public IEnumerable<ProductDTO> Products { get; set; } = new List<ProductDTO>();
    }
}
