﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EcommerceAPI.Domain
{
    public class ShoppingCart
    {
        public int Count { get; set; } = 1;
        public string ProductId { get; set; } = null!;
        [ValidateNever, ForeignKey("ProductId")] public Product? Product { get; set; }
        [ForeignKey("ApplicationUser")] public required string ApplicationUserId { get; set; }
        [ValidateNever, JsonIgnore] public ApplicationUser? ApplicationUser { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
