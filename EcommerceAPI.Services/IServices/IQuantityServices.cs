using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAPI.Services.IServices
{
    /// <summary>
    /// Service interface for managing product quantities.
    /// </summary>
    public interface IQuantityServices
    {
        /// <summary>
        /// Add or update the specified quantity to the product's availability.
        /// </summary>
        /// <param name="quantity">The quantity to add. Must be a positive value greater than zero.</param>
        /// <param name="productId">The ID of the product. Must not be null or empty.</param>
        public Task ModifyQuantityAsync(int quantity, string productId);
    }
}
