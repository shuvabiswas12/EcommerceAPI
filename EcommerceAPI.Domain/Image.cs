using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EcommerceAPI.Domain
{
    public class Image
    {
        [Key] public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required(ErrorMessage = "Url of image should not be empty!")] public required string ImageUrl { get; set; }
        public string ProductId { get; set; } = null!;
        [ValidateNever, JsonIgnore, ForeignKey("ProductId")] public Product Product { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
