﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAPI.Utilities
{
    public class TrackingIdGenerator
    {
        // Response should like: TRK-10digits
        public static string GenerateTrackingId()
        {
            // Generate a unique tracking ID using a GUID
            // Use a hash of a new GUID to generate a semi-unique number
            Guid guid = Guid.NewGuid();
            int hash = guid.GetHashCode(); // May be negative

            // Testing explain
            //var tempHash = hash % 1000000000;  // 9 digits -> 9's 0 means 9's digit
            //var tempHash3 = hash % 1_000_000_0000;  // 10 digits -> 10's 0 means 10's digit

            // Make it absolute and fit into 10 digits
            long number = Math.Abs((long)hash) % 1_000_000_0000; // Max 10 digits

            // Pad with zeros to ensure 10 digits
            return $"TRK-{number:D10}";
        }
    }
}
