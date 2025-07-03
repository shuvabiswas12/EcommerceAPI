using EcommerceAPI.DTOs;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAPI.Services.IServices
{
    /// <summary>
    /// Provides payment-related services.
    /// </summary>
    public interface IPaymentServices
    {
        /// <summary>
        /// Creates a payment intent asynchronously.
        /// </summary>
        /// <param name="amount">The amount for the payment intent.</param>
        /// <returns>The client secret for the payment intent.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the amount is zero.</exception>
        public Task<PaymentIntentDTO> CreatePaymentIntentAsync(long amount);
    }
}
