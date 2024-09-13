using Microsoft.AspNetCore.Http.HttpResults;
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
    public class FeaturedProductServices : IFeaturedProductServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public FeaturedProductServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddProductAsFeatured(FeaturedProductRequestDTO featuredProductRequestDTO)
        {
            var isFeaturedAlready = await _unitOfWork.GenericRepository<FeaturedProduct>()
                .GetTAsync(p => p.ProductId == featuredProductRequestDTO.Id);

            if (isFeaturedAlready != null)
            {
                throw new DuplicateEntriesException(message: "The product already added as a featured product.");
            }

            var result = await _unitOfWork.GenericRepository<FeaturedProduct>().AddAsync(new FeaturedProduct { ProductId = featuredProductRequestDTO.Id });
            if (result == null)
            {
                throw new InvalidOperationException(message: "There was a problem while creating product as a featured product.");
            }
            await _unitOfWork.SaveAsync();
        }

        public async Task RemoveProductFromFeatured(FeaturedProductRequestDTO featuredProductRequestDTO)
        {
            var isFearuredProductFound = await _unitOfWork.GenericRepository<FeaturedProduct>().GetTAsync(p => p.ProductId == featuredProductRequestDTO.Id);
            if (isFearuredProductFound == null)
            {
                throw new NotFoundException(message: "Product is not found as a featured product.");
            }
            await _unitOfWork.GenericRepository<FeaturedProduct>().DeleteAsync(new FeaturedProduct { ProductId = featuredProductRequestDTO.Id });
            await _unitOfWork.SaveAsync();
        }
    }
}
