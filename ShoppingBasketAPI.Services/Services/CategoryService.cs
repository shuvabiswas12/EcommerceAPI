using ShoppingBasketAPI.Data.UnitOfWork;
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

        public Task<IEnumerable<CategoryResponseDTO>> GetAllCategories()
        {
            throw new NotImplementedException();
        }

        public Task<CategoryResponseDTO> GetCategoryById(object id)
        {
            throw new NotImplementedException();
        }

        public Task<CategoryResponseDTO> UpdateCategory(object id, string categoryName)
        {
            throw new NotImplementedException();
        }
    }
}
