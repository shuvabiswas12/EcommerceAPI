using ShoppingBasketAPI.Data.UnitOfWork;
using ShoppingBasketAPI.Domain;
using ShoppingBasketAPI.DTOs;
using ShoppingBasketAPI.DTOs.GenericResponse;
using ShoppingBasketAPI.Services.IServices;
using ShoppingBasketAPI.Utilities.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasketAPI.Services.Services
{
    public class CategoryServices : ICategoryServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task DeleteCategory(object id)
        {
            var dataToDelete = await _unitOfWork.GenericRepository<Category>().GetTAsync(predicate: x => x.Id == id.ToString());
            if (dataToDelete == null)
            {
                throw new NotFoundException(message: "Category Not Found.");
            }
            await _unitOfWork.GenericRepository<Category>().DeleteAsync(dataToDelete);
            await _unitOfWork.SaveAsync();
        }

        public async Task<GenericResponseDTO<Category>> GetAllCategories()
        {
            var result = await _unitOfWork.GenericRepository<Category>().GetAllAsync();
            var categoriesResponse = new GenericResponseDTO<Category>
            {
                Data = result,
                Count = result.Count()
            };
            return categoriesResponse;
        }

        public async Task<Category> GetCategoryById(object id)
        {
            var category = await _unitOfWork.GenericRepository<Category>().GetTAsync(predicate: x => x.Id == id.ToString());
            if (category == null)
                throw new NotFoundException(message: "Category not found.");
            return category;
        }

        public async Task<Category> UpdateCategory(object id, string categoryName)
        {
            var dataToUpdate = await _unitOfWork.GenericRepository<Category>().GetTAsync(predicate: x => x.Id == id.ToString());
            if (dataToUpdate == null)
            {
                throw new NotFoundException(message: "Category not found.");
            }
            dataToUpdate.Name = categoryName;
            await _unitOfWork.SaveAsync();
            return dataToUpdate;
        }

        public async Task<Category> CreateCategory(CategoryCreateRequestDTO createCategory)
        {
            Category category = new Category { Name = createCategory.Name.Trim() };
            var createdCategory = await _unitOfWork.GenericRepository<Category>().AddAsync(category);
            await _unitOfWork.SaveAsync();
            return createdCategory;
        }
    }
}
