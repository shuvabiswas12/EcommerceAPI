using EcommerceAPI.Data.UnitOfWork;
using EcommerceAPI.Domain;
using EcommerceAPI.DTOs;
using EcommerceAPI.DTOs.GenericResponse;
using EcommerceAPI.Services.IServices;
using EcommerceAPI.Utilities.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAPI.Services.Services
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
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id), "Category ID must be provided.");
            }
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
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id), "Category ID must be provided.");
            }
            var category = await _unitOfWork.GenericRepository<Category>().GetTAsync(predicate: x => x.Id == id.ToString());
            return category == null ? throw new NotFoundException(message: "Category not found.") : category;
        }

        public async Task<Category> UpdateCategory(object id, string categoryName)
        {
            if (id == null || string.IsNullOrWhiteSpace(categoryName))
            {
                throw new ArgumentNullException("ID and category name must be provided.");
            }
            var dataToUpdate = await _unitOfWork.GenericRepository<Category>().GetTAsync(predicate: x => x.Id == id.ToString());
            if (dataToUpdate == null)
            {
                throw new NotFoundException(message: "Category not found.");
            }
            dataToUpdate.Name = categoryName;
            await _unitOfWork.SaveAsync();
            return dataToUpdate;
        }

        public async Task<Category> CreateCategory(CategoryCreateDTO createCategory)
        {
            if (string.IsNullOrWhiteSpace(createCategory.Name))
            {
                throw new ArgumentException("Category name cannot be empty.");
            }
            Category category = new Category { Name = createCategory.Name.Trim() };
            var createdCategory = await _unitOfWork.GenericRepository<Category>().AddAsync(category);
            await _unitOfWork.SaveAsync();
            return createdCategory;
        }
    }
}
