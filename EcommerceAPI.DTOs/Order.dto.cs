using EcommerceAPI.Domain;
using EcommerceAPI.Utilities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EcommerceAPI.DTOs
{
    public class OrderDTO
    {
        public string Id { get; set; }
        [DisplayName("PaymentId")] public string PaymentIntentId { get; set; }
        public string UserId { get; set; } = null!;
        [JsonIgnore] public DateTime OrderDate { get; set; }
        public long OrderTimestamp => Utilities.Timestamps.GetTimestamp(OrderDate);
        public decimal OrderAmount { get; set; }
        public string Currency { get; set; }
        public string OrderStatus { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentType { get; set; }
        public string TrackingNumber { get; set; }
        public OrderAddressDTO ShippingAddress { get; set; }
        public List<OrderDetailsDTO> OrderDetails { get; set; }
    }

    public class OrderAddressDTO
    {
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string? AlternatePhone { get; set; }
        public string Email { get; set; }
        public string HouseName { get; set; }
        public string? RoadNumber { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string? Province { get; set; }
        public string? State { get; set; }
        public string PostCode { get; set; }
        public string? LandMark { get; set; }
        public string AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
    }

    public class OrderDetailsDTO
    {
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        [Precision(10, 2)] public decimal Price { get; set; }
        [Precision(10, 2)] public decimal Total => Quantity * Price;
    }

    public class OrderUpdateDTO
    {
        public OrdersStatus? orderStatus { get; set; }
    }
}
