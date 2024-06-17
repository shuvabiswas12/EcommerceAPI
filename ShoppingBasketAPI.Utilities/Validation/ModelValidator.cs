using Microsoft.AspNetCore.Mvc.ModelBinding;
using ShoppingBasketAPI.Utilities.Validation.CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasketAPI.Utilities.Validation
{
    public class ModelValidator
    {
        public static ModelStateDictionary ValidateModel(object model)
        {
            var errors = new ModelStateDictionary();
            var properties = model.GetType().GetProperties();

            foreach (var property in properties)
            {
                var value = property.GetValue(model);

                foreach (var attribute in property.GetCustomAttributes(true))
                {
                    if (attribute is NotEmptyAttribute requiredAttribute)
                    {
                        if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                        {
                            errors.AddModelError(property.Name, requiredAttribute.ErrorMessage);
                        }
                    }
                    else if (attribute is GreaterThanAttribute greaterThanAttribute)
                    {
                        if (value is decimal decimalValue)
                        {
                            if (decimalValue <= (decimal)greaterThanAttribute.Value)
                            {
                                errors.AddModelError(property.Name, greaterThanAttribute.ErrorMessage);
                            }
                        }
                    }
                    else if (attribute is MinimumOneImageUrlAttribute minimumOneImageUrlAttribute)
                    {
                        if (value is ICollection<string> imageUrls && !imageUrls.Any(u => !string.IsNullOrWhiteSpace(u)))
                        {
                            errors.AddModelError(property.Name, minimumOneImageUrlAttribute.ErrorMessage);
                        }
                    }
                }
            }
            return errors;
        }
    }
}
