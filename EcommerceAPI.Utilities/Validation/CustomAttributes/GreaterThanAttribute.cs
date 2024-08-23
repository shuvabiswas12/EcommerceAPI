using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAPI.Utilities.Validation.CustomAttributes
{
    public class GreaterThanAttribute : Attribute
    {
        public double Value { get; set; }
        public string ErrorMessage { get; set; }

        public GreaterThanAttribute(double value, string errorMessage)
        {
            Value = value;
            ErrorMessage = errorMessage;
        }
    }
}
