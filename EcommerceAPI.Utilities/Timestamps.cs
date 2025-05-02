using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAPI.Utilities
{
    public class Timestamps
    {
        /// <summary>
        /// Takes a DateTime object and returns a UnixTimeSeconds as Timestamp.
        /// </summary>
        public static long GetTimestamp(DateTime dateTime)
        {
            return ((DateTimeOffset)dateTime).ToUnixTimeSeconds();
        }
    }
}
