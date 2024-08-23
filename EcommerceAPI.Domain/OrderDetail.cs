using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace EcommerceAPI.Domain
{
    public class OrderDetail
    {
        [Required]
        public string OrderHeaderId { get; set; } = null!;

        [ForeignKey("OrderHeaderId"), ValidateNever]
        public OrderHeader OrderHeader { get; set; } = null!;

        [Required]
        public string ProductId { get; set; } = null!;

        [ValidateNever, ForeignKey("ProductId")]
        public Product Product { get; set; } = null!;

        [Required]
        public string ProductName { get; set; } = null!;

        [Required]
        public int Quantity { get; set; }

        [Required, Precision(10, 2)]
        public decimal Price { get; set; }

        [Required, Precision(10, 2)]
        public decimal Total => Quantity * Price;
    }

}
