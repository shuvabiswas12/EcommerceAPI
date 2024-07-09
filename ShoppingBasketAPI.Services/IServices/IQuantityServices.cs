using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasketAPI.Services.IServices
{
    public interface IQuantityServices
    {
        public Task AddQuantityAsync(int quantity, string productId);

        public Task ReduceQuantityAsync(string productId, int quantity = 0);
    }
}
