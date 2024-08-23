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
        public required string ProductId { get; set; }

        [Required] public required int Availability { get; set; } = 0;

        public DateTime AddedAt { get; set; } = DateTime.Now;

        [ForeignKey("ProductId"), ValidateNever, JsonIgnore] public Product Product { get; set; } = new Product();
    }
}
