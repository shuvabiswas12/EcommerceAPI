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
        /// Adds the specified quantity to the product's availability.
        /// </summary>
        /// <param name="quantity">The quantity to add. Must be a positive value greater than zero.</param>
        /// <param name="productId">The ID of the product. Must not be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="ArgumentException">Thrown when the quantity is less than or equal to zero. Or When the productId is null or empty.</exception>
        public Task AddQuantityAsync(int quantity, string productId);

        /// <summary>
        /// Reduces the specified quantity from the product's availability.
        /// </summary>
        /// <param name="productId">The ID of the product. Must not be null or empty.</param>
        /// <param name="quantity">The quantity to reduce. Must be a positive value or zero. If zero, the quantity will be reduced by one.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="ArgumentException">Thrown when the quantity is less than zero or if the resulting quantity is invalid. Or when the productId is null or empty.</exception>
        /// <exception cref="NotFoundException">Thrown when no quantity is listed for the specified product.</exception>
        public Task ReduceQuantityAsync(string productId, int quantity = 0);
    }
}
