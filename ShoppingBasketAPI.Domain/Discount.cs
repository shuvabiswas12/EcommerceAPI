using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ShoppingBasketAPI.Domain
{
    public class Discount
    {
        public string ProductId { get; set; } = null!;

        [ValidateNever, JsonIgnore, ForeignKey("ProductId")]
        public Product Product { get; set; } = null!;

        // Should be provide 20 if rate percent is 20%
        [Required(ErrorMessage = "Discount rate without percent value. Example:- Write 20 if discount rate is 20%.")]
        public double DiscountRate { get; set; } = 0.0;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}