using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasketAPI.Utilities.Validation.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class NotEmptyAttribute : Attribute
    {
        public string ErrorMessage { get; set; }

        public NotEmptyAttribute(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
