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

        public async Task DeleteCategory(object id)
        {
            var dataToDelete = await _unitOfWork.GenericRepository<Category>().GetTAsync(predicate: x => x.Id == id.ToString());
            if (dataToDelete == null)
            {
                throw new Exception(message: "Data Not Found.");
            }
            await _unitOfWork.GenericRepository<Category>().DeleteAsync(dataToDelete);
            await _unitOfWork.SaveAsync();
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
            return await _unitOfWork.GenericRepository<Category>().GetTAsync(predicate: x => x.Id == id.ToString());
        }

        public async Task<Category> UpdateCategory(object id, string categoryName)
        {
            var dataToUpdate = await _unitOfWork.GenericRepository<Category>().GetTAsync(predicate: x => x.Id == id.ToString());
            if (dataToUpdate == null)
            {
                throw new Exception(message: "Data not found.");
            }
            dataToUpdate.Name = categoryName;
            await _unitOfWork.SaveAsync();
            return dataToUpdate;
        }
    }
}
