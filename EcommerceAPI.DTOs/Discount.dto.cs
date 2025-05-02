using EcommerceAPI.Utilities.Validation.CustomAttributes;
using EcommerceAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace EcommerceAPI.DTOs
{
    public class DiscountRequestDTO
    {
        [NotEmpty("DiscountRate is required.")]
        public double DiscountRate { get; set; }
        [NotEmpty("Discount End Date is required.")] public DateTime DiscountEndAt { get; set; }
    }

    public class DiscountDTO
    {
        public double DiscountRate { get; set; }
        public Boolean DiscountEnabled { get; set; }
        public long? DiscountStartTimestamp { get; set; }
        public long? DiscountEndTimestamp { get; set; }
        [JsonIgnore]
        public DateTime? DiscountEndAt
        {
            set
            {
                DiscountEndTimestamp = value.HasValue ? Utilities.Timestamps.GetTimestamp(value.Value) : null;
            }
        }

        [JsonIgnore]
        public DateTime? DiscountStartAt
        {
            set
            {
                DiscountStartTimestamp = value.HasValue ? Utilities.Timestamps.GetTimestamp(value.Value) : null;
            }
        }
    }
}
