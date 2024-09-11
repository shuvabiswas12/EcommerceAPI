using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAPI.Utilities.Exceptions
{
    public class DuplicateEntriesException : Exception
    {
        public DuplicateEntriesException(string? message) : base(message)
        {
        }
    }
}
