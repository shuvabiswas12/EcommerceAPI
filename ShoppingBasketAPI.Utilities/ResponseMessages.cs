using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasketAPI.Utilities
{
    public static class ResponseMessages
    {
        public static string StatusCode_200_DeleteMessage { get; private set; } = "Deleted";
        public static string StatusCode_200_CreateMessage { get; private set; } = "Created";
        public static string StatusCode_200_UpdateMessage { get; private set; } = "Updated";
        public static string StatusCode_500_ErrorMessage { get; private set; } = "An error occured while processing your request";
    }
}
