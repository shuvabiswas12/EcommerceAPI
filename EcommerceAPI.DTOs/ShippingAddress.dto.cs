using EcommerceAPI.Utilities.Validation.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAPI.DTOs
{
    public class ShippingAddressDTO
    {
        [NotEmpty(errorMessage: "Fullname must be given.")] public string FullName { get; set; } = null!;
        [NotEmpty(errorMessage: "Phone no must be given.")] public string Phone { get; set; } = null!;
        public string? AlternatePhone { get; set; }
        [NotEmpty(errorMessage: "Email must be given.")] public string Email { get; set; } = null!;
        [NotEmpty(errorMessage: "House name must be given.")] public string HouseName { get; set; } = null!;
        public string? RoadNumber { get; set; }
        public string? LandMark { get; set; }
        [NotEmpty(errorMessage: "City must be given.")] public string City { get; set; } = null!;
        [NotEmpty(errorMessage: "Country must be given.")] public string Country { get; set; } = null!;
        public string? Province { get; set; }
        public string? State { get; set; }
        [NotEmpty(errorMessage: "Post code must be given.")] public string PostCode { get; set; } = null!;
        public string AddressLine1 { get; set; } = null!;
        public string? AddressLine2 { get; set; }
    }
}
