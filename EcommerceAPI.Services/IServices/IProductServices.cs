using EcommerceAPI.Domain;
using EcommerceAPI.DTOs;
using EcommerceAPI.DTOs.GenericResponse;
using EcommerceAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAPI.Services.IServices
{
    public interface IProductServices
    {
        public Task<IEnumerable<Product>> GetAllProduct(string? name = null, string? category = null, PriceFilter? price = PriceFilter.LowToHigh, bool? discount = false);
        public Task<Product> GetProductById(object id);
        public Task DeleteProduct(object id);
        public Task UpdateProduct(Object id, ProductUpdateDTO productDto);
        public Task<String> CreateProduct(ProductCreateDTO productDto);
    }
}
