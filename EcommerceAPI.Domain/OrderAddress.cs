using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAPI.Domain
{
    public class OrderAddress
    {
        [Key] public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required] public required string FullName { get; set; } = null!;
        [Required] public required string Phone { get; set; } = null!;
        public string? AlternatePhone { get; set; }
        [Required] public required string Email { get; set; } = null!;
        [Required] public required string HouseName { get; set; } = null!;
        [Required] public required string RoadNumber { get; set; } = null!;
        [Required] public required string City { get; set; } = null!;
        [Required] public required string Country { get; set; } = null!;
        [Required] public required string Province { get; set; } = null!;
        [Required] public required string State { get; set; } = null!;
        [Required] public required string PostCode { get; set; } = null!;
        public string? LandMark { get; set; }
        public string AddressLine1 { get; set; } = null!;
        public string? AddressLine2 { get; set; }
        public OrderHeader OrderHeader { get; set; } = null!;
    }
}
