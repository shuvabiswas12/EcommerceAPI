using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasketAPI.Utilities.Exceptions
{
    public class DuplicateEntriesFoundException : Exception
    {
        public DuplicateEntriesFoundException(string? message) : base(message)
        {
        }
    }
}
