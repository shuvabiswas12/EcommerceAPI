using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using EcommerceAPI.Utilities;
using EcommerceAPI.Utilities.Validation.CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace EcommerceAPI.Domain
{
    public class OrderHeader
    {
        [Key] public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required] public required string PaymentIntentId { get; set; }
        [Required] public required string ApplicationUserId { get; set; } = null!;
        [ForeignKey("ApplicationUserId"), ValidateNever] public ApplicationUser ApplicationUser { get; set; } = null!;
        [Required] public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        [Required] public required double OrderAmount { get; set; }
        [Required] public required string Currency { get; set; }
        [Required] public required string OrderStatus { get; set; } = OrdersStatus.Pending.ToString();
        public string? PaymentStatus { get; set; }
        public string? PaymentType { get; set; }
        public string? TrackingNumber { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        [ForeignKey("OrderAddressId"), ValidateNever] public OrderAddress OrderAddress { get; set; } = null!;
        [JsonIgnore] public string OrderAddressId { get; set; } = null!;
    }
}
