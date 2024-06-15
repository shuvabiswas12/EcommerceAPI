using ShoppingBasketAPI.Data.UnitOfWork;
using ShoppingBasketAPI.Domain;
using ShoppingBasketAPI.DTOs;
using ShoppingBasketAPI.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasketAPI.Services.Services
{
    public class CategoryService : ICategoryServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task DeleteCategory(object id)
        {
            throw new NotImplementedException();
        }

        public async Task<CategoryResponseDTO> GetAllCategories()
        {
            var result = await _unitOfWork.GenericRepository<Category>().GetAllAsync();
            var categories = new CategoryResponseDTO
            {
                Categories = result,
                TotalCategories = result.Count()
            };
            return categories;
        }

        public async Task<Category> GetCategoryById(object id)
        {
            if (!Guid.TryParse(id.ToString(), out _))
            {
                throw new Exception(message: "Given ID should be a valid.");
            }
            return await _unitOfWork.GenericRepository<Category>().GetTAsync(predicate: x => x.Id == id.ToString());
        }

        public Task<CategoryResponseDTO> UpdateCategory(object id, string categoryName)
        {
            throw new NotImplementedException();
        }
    }
}
