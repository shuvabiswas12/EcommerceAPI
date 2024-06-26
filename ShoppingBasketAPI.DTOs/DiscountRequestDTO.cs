using ShoppingBasketAPI.Utilities.Validation.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasketAPI.DTOs
{
    public class DiscountRequestDTO
    {
        [NotEmpty("DiscountRate is required.")]
        public double DiscountRate { get; set; }
    }
}
