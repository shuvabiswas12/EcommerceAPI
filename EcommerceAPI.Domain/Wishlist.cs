﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceAPI.Domain
{
    public class Wishlist
    {
        [Required(ErrorMessage = "User id required.")] public required string ApplicationUserId { get; set; }
        [ValidateNever, ForeignKey("ApplicationUserId"), JsonIgnore] public ApplicationUser ApplicationUser { get; set; } = null!;
        [Required(ErrorMessage = "Product id required.")] public required string ProductId { get; set; }
        [ValidateNever, ForeignKey("ProductId"), JsonIgnore] public Product Product { get; set; } = null!;
    }
}
