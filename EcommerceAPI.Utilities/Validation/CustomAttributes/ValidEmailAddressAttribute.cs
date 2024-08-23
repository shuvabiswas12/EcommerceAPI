using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAPI.Utilities.Validation.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ValidEmailAddressAttribute : Attribute
    {
        public string ErrorMessage { get; set; }
        public ValidEmailAddressAttribute(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
