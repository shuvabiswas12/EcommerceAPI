using Microsoft.AspNetCore.Mvc.ModelBinding;
using ShoppingBasketAPI.Utilities.Validation.CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
                    else if (attribute is ValidEmailAddressAttribute validEmailAddressAttribute)
                    {
                        // Regex pattern for validating email addresses from Gmail, Yahoo, or Hotmail
                        string pattern = @"^[a-zA-Z0-9._%+-]+@(gmail|yahoo|hotmail)\.(com|co\.uk)$";
                        Regex regex = new Regex(pattern);
                        var isValid = regex.IsMatch(value!.ToString()!);
                        if (!isValid)
                        {
                            errors.AddModelError(property.Name, errorMessage: validEmailAddressAttribute.ErrorMessage);
                        }
                    }
                    else if (attribute is ValidPasswordAttribute validPasswordAttribute)
                    {
                        /***
                         * Password must be at least 8 characters long and contain at least one uppercase letter,
                         * one lowercase letter, one digit, and one special symbol (@, #, $).
                         */
                        string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@#$]).{8,}$";
                        Regex regex = new Regex(pattern);
                        var isValid = regex.IsMatch(value!.ToString()!);
                        if (!isValid)
                        {
                            errors.AddModelError(property.Name, errorMessage: validPasswordAttribute.ErrorMessage);
                        }
                    }
                }
            }
            return errors;
        }

        public static Dictionary<string, string[]> GetErrors(ModelStateDictionary modelState)
        {
            var errors = modelState
                    .Where(ms => ms.Value!.Errors.Any())
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                    );
            return errors;
        }
    }
}