using EcommerceAPI.Utilities.Validation;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAPI.Utilities.Exceptions
{
    public class ModelValidationException : Exception
    {
        public Dictionary<string, string[]> ValidationErrors;
        private static string ValidationTitle = "Data validation failed.";

        public ModelValidationException(ModelStateDictionary modelState) : base(ValidationTitle)
        {
            ValidationErrors = ModelValidator.GetErrors(modelState);
        }

        public ModelValidationException(string key, string[] message)
            : base(ValidationTitle)
        {
            ValidationErrors = new Dictionary<string, string[]> { { key, message } };
        }
    }
}
