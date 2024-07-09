using Microsoft.IdentityModel.Tokens;
using ShoppingBasketAPI.Data.UnitOfWork;
using ShoppingBasketAPI.Domain;
using ShoppingBasketAPI.Services.IServices;
using ShoppingBasketAPI.Utilities.Exceptions;
using Stripe.Climate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasketAPI.Services.Services
{
    public class QuantityServices : IQuantityServices
    {

        private readonly IUnitOfWork _unitOfWork;

        public QuantityServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddQuantityAsync(int quantity, string productId)
        {
            if (quantity <= 0) throw new ArgumentException(message: "Quantity must be positive value and that of greater then zero.");

            if (string.IsNullOrEmpty(productId)) throw new ArgumentException("ProductId", "Product id must be mentioned.");

            var newAvailability = new ProductAvailability { Availability = quantity, ProductId = productId };
            await _unitOfWork.GenericRepository<ProductAvailability>().AddAsync(newAvailability);
            await _unitOfWork.SaveAsync();
            return;
        }

        public async Task ReduceQuantityAsync(string productId, int quantity = 0)
        {
            if (string.IsNullOrEmpty(productId)) throw new ArgumentException("ProductId", "Product id must be mentioned.");

            if (quantity < 0) throw new ArgumentException(message: "Quantity must be positive value.");

            var currentQuantity = await _unitOfWork.GenericRepository<ProductAvailability>().GetTAsync(pa => pa.ProductId == productId);
            if (currentQuantity == null)
            {
                throw new NotFoundException(message: "No qunatity is listed.");
            }

            if (currentQuantity.Availability < quantity)
                throw new ArgumentException(message: "Invalid product quantity.");

            currentQuantity.Availability = (int)(currentQuantity.Availability - (quantity == 0 ? 1 : quantity));
            await _unitOfWork.SaveAsync();
        }
    }
}
