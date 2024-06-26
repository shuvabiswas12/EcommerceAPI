using ShoppingBasketAPI.Data.UnitOfWork;
using ShoppingBasketAPI.Domain;
using ShoppingBasketAPI.DTOs;
using ShoppingBasketAPI.Services.IServices;
using ShoppingBasketAPI.Utilities.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasketAPI.Services.Services
{
    public class DiscountServices : IDiscountServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public DiscountServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddDiscount(string id, DiscountRequestDTO discountRequestDTO)
        {
            var isDiscountFound = await _unitOfWork.GenericRepository<Discount>().GetTAsync(d => d.ProductId == id);

            if (isDiscountFound == null)
            {
                // Create a new discount.
            }
            else
            {
                // Update the old discount.
            }
        }

        public async Task RemoveDiscount(string id, DiscountRequestDTO discountRequestDTO)
        {
            var isDiscountFound = await _unitOfWork.GenericRepository<Discount>().GetTAsync(d => d.ProductId == id);
            if (isDiscountFound == null)
            {
                throw new NotFoundException(message: "There is no discount available for the product.");
            }

            // Remove the discount.
        }
    }
}
