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

        [NotEmpty(errorMessage: "Road number must be given.")] public string RoadNumber { get; set; } = null!;

        public string? LandMark { get; set; }

        [NotEmpty(errorMessage: "City must be given.")] public string City { get; set; } = null!;

        [NotEmpty(errorMessage: "Country must be given.")] public string Country { get; set; } = null!;

        [NotEmpty(errorMessage: "Province or Dictrict must be given.")] public string Province { get; set; } = null!;

        [NotEmpty(errorMessage: "State must be given.")] public string State { get; set; } = null!;

        [NotEmpty(errorMessage: "Post code must be given.")] public string PostCode { get; set; } = null!;

        public string? AddressLine1 { get; set; }

        public string? AddressLine2 { get; set; }
    }
}
