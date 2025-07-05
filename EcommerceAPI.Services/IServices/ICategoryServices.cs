using EcommerceAPI.Domain;
using EcommerceAPI.DTOs;
using EcommerceAPI.DTOs.GenericResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAPI.Services.IServices
{
    public interface ICategoryServices
    {
        public Task<IEnumerable<Category>> GetAllCategories();
        public Task<Category> GetCategoryById(object id);
        public Task DeleteCategory(object id);
        public Task UpdateCategory(object id, string categoryName);
        public Task<string> CreateCategory(CategoryCreateDTO createCategory);
    }
}
