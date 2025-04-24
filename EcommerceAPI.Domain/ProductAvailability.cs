using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EcommerceAPI.Domain
{
    public class ProductAvailability
    {
        [Required] public required int Availability { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public int? LastAvailability { get; set; }
        public required string ProductId { get; set; }
        [ForeignKey("ProductId"), ValidateNever, JsonIgnore] public Product Product { get; set; } = null!;
    }
}
