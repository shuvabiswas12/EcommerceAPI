using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAPI.Utilities
{
    public class TrackingNumberGenerator
    {
        private static readonly Random _random = new Random();

        public static string GenerateTrackingNumber()
        {
            // Generate a random number
            int randomNumber = _random.Next(1000, 9999);

            // Combine the current date and time with the random number
            string trackingNumber = $"{DateTime.UtcNow:yyyyMMddHHmmss}-{randomNumber}";

            return trackingNumber;
        }
    }
}
