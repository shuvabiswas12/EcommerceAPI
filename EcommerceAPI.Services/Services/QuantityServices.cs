using Microsoft.IdentityModel.Tokens;
using EcommerceAPI.Data.UnitOfWork;
using EcommerceAPI.Domain;
using EcommerceAPI.Services.IServices;
using EcommerceAPI.Utilities.Exceptions;
using Stripe.Climate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAPI.Services.Services
{
    public class QuantityServices : IQuantityServices
    {

        private readonly IUnitOfWork _unitOfWork;

        public QuantityServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task ModifyQuantityAsync(int quantity, string productId)
        {
            var currentQuantity = await _unitOfWork.GenericRepository<ProductAvailability>()
                .GetTAsync(pa => pa.ProductId == productId);

            if (currentQuantity == null)
            {
                var newAvailability = new ProductAvailability { Availability = quantity, ProductId = productId };
                await _unitOfWork.GenericRepository<ProductAvailability>().AddAsync(newAvailability);
            }
            else
            {
                currentQuantity.Availability = (int)(currentQuantity.Availability - (quantity == 0 ? 1 : quantity));
            }
            await _unitOfWork.SaveAsync();
        }
    }
}
