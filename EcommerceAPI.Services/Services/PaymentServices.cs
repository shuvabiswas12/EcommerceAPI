using EcommerceAPI.DTOs;
using EcommerceAPI.Services.IServices;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAPI.Services.Services
{
    public class PaymentServices : IPaymentServices
    {
        public async Task<PaymentIntentDTO> CreatePaymentIntentAsync(long amount)
        {
            if (amount == 0) throw new ArgumentNullException("Payment amount should not be empty.");

            var options = new PaymentIntentCreateOptions
            {
                Amount = amount,
                Currency = "usd",
                PaymentMethodTypes = new List<string> { "card" },

                // Only for local testing
                PaymentMethod = "pm_card_visa",
                Confirm = true,  // auto confirm at creation time
            };
            var service = new PaymentIntentService();
            var paymentIntent = await service.CreateAsync(options);
            return new PaymentIntentDTO { PaymentIntentId = paymentIntent.Id, PaymentIntentSecret = paymentIntent.ClientSecret };
        }
    }
}
