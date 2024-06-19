using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ShoppingBasketAPI.Domain
{
    public class FeaturedProduct
    {
        public string ProductId { get; set; } = null!;

        [ValidateNever, JsonIgnore, ForeignKey("ProductId")]
        public Product Product { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}