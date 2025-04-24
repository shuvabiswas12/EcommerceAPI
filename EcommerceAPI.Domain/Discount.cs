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
    public class Discount
    {
        [Key] public string Id { get; set; } = Guid.NewGuid().ToString();
        public string ProductId { get; set; } = null!;
        [ValidateNever, JsonIgnore, ForeignKey("ProductId")] public Product Product { get; set; } = null!;

        [Required(ErrorMessage = "Discount rate without percent value. Example:- Write 20 if discount rate is 20%.")]
        public required double DiscountRate { get; set; } = 0.0;
        public required Boolean DiscountEnabled { get; set; } = true;
        public DateTime? DiscountEndAt { get; set; }
        public DateTime DiscountStartAt { get; set; } = DateTime.UtcNow;
    }
}