using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAPI.Utilities
{
    public enum OrdersStatus
    {
        Pending,
        Accepted,
        Canceled,
        Preparing,
        Shiifted,
        Delivered,
        Returned,
    }

    public enum PaymentStatus
    {
        Due,
        Paid
    }

    public enum PaymentType
    {
        CashOnDelivery,
        OnlinePayment
    }

    public enum SortBy
    {
        DateASC,
        DateDESC,
        AmountASC,
        AmountDESC,
    }
}
