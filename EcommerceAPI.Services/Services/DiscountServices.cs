using EcommerceAPI.Data.UnitOfWork;
using EcommerceAPI.Domain;
using EcommerceAPI.DTOs;
using EcommerceAPI.Services.IServices;
using EcommerceAPI.Utilities.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAPI.Services.Services
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
            var discount = await _unitOfWork.GenericRepository<Discount>().GetTAsync(d => d.ProductId == id);

            if (discount == null)
            {
                // Create a new discount.
                await _unitOfWork.GenericRepository<Discount>().AddAsync(new Discount { ProductId = id, DiscountRate = discountRequestDTO.DiscountRate });
                await _unitOfWork.SaveAsync();
                await Task.CompletedTask;
                return;
            }
            else
            {
                // Update the old discount.
                discount.DiscountRate = discountRequestDTO.DiscountRate;
                await _unitOfWork.SaveAsync();
                await Task.CompletedTask;
                return;
            }
        }

        public async Task RemoveDiscount(string id)
        {
            var discountToRemove = await _unitOfWork.GenericRepository<Discount>().GetTAsync(d => d.ProductId == id);
            if (discountToRemove == null)
            {
                throw new NotFoundException(message: "There is no discount available for the product.");
            }

            // Remove the discount.
            await _unitOfWork.GenericRepository<Discount>().DeleteAsync(discountToRemove);
            await _unitOfWork.SaveAsync();
            await Task.CompletedTask;
            return;
        }
    }
}
