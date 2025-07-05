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
    public interface IProductServices
    {
        public Task<IEnumerable<Product>> GetAllProduct();
        public Task<Product> GetProductById(object id);
        public Task DeleteProduct(object id);
        public Task UpdateProduct(Object id, ProductUpdateDTO productDto);
        public Task<String> CreateProduct(ProductCreateDTO productDto);
    }
}
