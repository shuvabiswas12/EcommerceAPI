using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasketAPI.Utilities
{
    public static class GlobalErrorMessage
    {
        public static string ErrorMessage { get; private set; } = "An error occured while processing your request.";
    }
}
