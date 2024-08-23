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

namespace EcommerceAPI.Domain
{
    public class OrderHeader
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required] public required string PaymentIntentId { get; set; }

        [Required]
        public required string ApplicationUserId { get; set; } = null!;

        [ForeignKey("ApplicationUserId"), ValidateNever]
        public ApplicationUser ApplicationUser { get; set; } = null!;

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Required]
        public required double OrderAmount { get; set; }

        [Required] public required string Currency { get; set; }

        [Required]
        public required string OrderStatus { get; set; } = Status.OrderStatus_Pending;

        public string? PaymentStatus { get; set; }

        public string? PaymentType { get; set; }

        [Required] public required string FullName { get; set; } = null!;

        [Required] public required string Phone { get; set; } = null!;

        public string? AlternatePhone { get; set; }

        [Required] public required string Email { get; set; } = null!;

        [Required] public required string HouseName { get; set; } = null!;

        [Required] public required string RoadNumber { get; set; } = null!;

        public string? LandMark { get; set; }

        [Required] public required string City { get; set; } = null!;

        [Required] public required string Country { get; set; } = null!;

        [Required] public required string Province { get; set; } = null!;

        [Required] public required string State { get; set; } = null!;

        [Required] public required string PostCode { get; set; } = null!;

        public string? TrackingNumber { get; set; }

        public string? AddressLine1 { get; set; }

        public string? AddressLine2 { get; set; }

        // Navigation property
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }

}
