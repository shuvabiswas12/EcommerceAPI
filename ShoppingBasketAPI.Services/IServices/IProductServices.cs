using ShoppingBasketAPI.Domain;
using ShoppingBasketAPI.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasketAPI.Services.IServices
{
    public interface IProductServices
    {
        public Task<ProductResponseDTO> GetAllProduct();
        public Task<Product> GetProductById(object id);
        public Task DeleteProduct(object id);
        public Task<Product> UpdateProduct(Product product);
        public Task<Product> CreateProduct(Product product);
    }
}
