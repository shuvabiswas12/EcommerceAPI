using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAPI.Utilities.Exceptions
{
    public class ApiException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public object ErrorDetails { get; } = null;
        public ApiException(HttpStatusCode statusCode, string message, object? errorDetails = null)
        : base(message)
        {
            StatusCode = statusCode;
            if (errorDetails is null && !string.IsNullOrEmpty(message))
            {
                ErrorDetails = new { error = message };
            }
        }
    }
}
