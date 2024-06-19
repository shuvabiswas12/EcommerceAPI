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
    public class ProductCategory
    {
        public string ProductId { get; set; } = null!;
        public string CategoryId { get; set; } = null!;

        [JsonIgnore, ValidateNever, ForeignKey("CategoryId")] public Category Category { get; set; } = null!;

        [JsonIgnore, ValidateNever, ForeignKey("ProductId")] public Product Product { get; set; } = null!;
    }
}
