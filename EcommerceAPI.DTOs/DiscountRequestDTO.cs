using EcommerceAPI.Utilities.Validation.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAPI.DTOs
{
    public class DiscountRequestDTO
    {
        [NotEmpty("DiscountRate is required.")]
        public double DiscountRate { get; set; }
        [NotEmpty("Discount End Date is required.")] public DateTime DiscountEndAt { get; set; }
    }
}
