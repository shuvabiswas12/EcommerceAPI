using ShoppingBasketAPI.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasketAPI.Services.IServices
{
    public interface ICategoryServices
    {
        public Task<IEnumerable<CategoryResponseDTO>> GetAllCategories();
        public Task<CategoryResponseDTO> GetCategoryById(object id);
        public Task DeleteCategory(object id);
        public Task<CategoryResponseDTO> UpdateCategory(object id, string categoryName);
    }
}
