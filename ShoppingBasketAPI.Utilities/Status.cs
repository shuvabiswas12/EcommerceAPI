using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasketAPI.Utilities
{
    public static class Status
    {
        public static string OrderStatus_Pending { get; private set; } = "Pending";
        public static string OrderStatus_Accepted { get; private set; } = "Accepted";
        public static string OrderStatus_Canceled { get; private set; } = "Canceled";
        public static string PaymentStatus_Due { get; private set; } = "DUE";
        public static string PaymentStatus_Paid { get; private set; } = "PAID";
        public static string PaymentType_CashOnDelivery { get; private set; } = "Cash-On-Delivery";
        public static string PaymentType_OnlinePayment { get; private set; } = "Online-Payment";
    }
}
