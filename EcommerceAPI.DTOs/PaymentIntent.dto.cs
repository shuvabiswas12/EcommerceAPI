using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAPI.DTOs
{
    public class PaymentIntentDTO
    {
        public string PaymentIntentId { get; set; }
        public string PaymentIntentSecret { get; set; }
    }
}
