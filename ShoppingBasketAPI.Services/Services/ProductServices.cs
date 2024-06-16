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
    public class ProductServices : IProductServices
    {
        private IUnitOfWork _unitOfWork;

        public ProductServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Product> CreateProduct(Product product)
        {
            if (product == null)
            {
                throw new Exception(message: "Could not create product.");
            }

            var createdProduct = await _unitOfWork.GenericRepository<Product>().AddAsync(product);
            await _unitOfWork.SaveAsync();
            return createdProduct;
        }

        public async Task DeleteProduct(object id)
        {
            var productToDelete = await _unitOfWork.GenericRepository<Product>().GetTAsync(x => x.Id == id.ToString(), includeProperties: "Images");
            if (productToDelete == null)
            {
                throw new Exception("Product not found.");
            }
            await _unitOfWork.GenericRepository<Product>().DeleteAsync(productToDelete);
            await _unitOfWork.SaveAsync();
        }

        public async Task<ProductResponseDTO> GetAllProduct()
        {
            var productsResult = await _unitOfWork.GenericRepository<Product>().GetAllAsync(includeProperties: "Images");
            var productsResponseDto = new ProductResponseDTO
            {
                products = productsResult,
                totalProducts = productsResult.Count()
            };
            return productsResponseDto;
        }

        public async Task<Product> GetProductById(object id)
        {
            var productResult = await _unitOfWork.GenericRepository<Product>().GetTAsync(x => x.Id == id.ToString(), includeProperties: "Images");
            return productResult;
        }

        public async Task<Product> UpdateProduct(object id, Product product)
        {
            var productToUpdate = await _unitOfWork.GenericRepository<Product>().GetTAsync(x => x.Id == id.ToString(), includeProperties: "Images");
            if (productToUpdate == null)
            {
                throw new Exception(message: "Product not found.");
            }
            productToUpdate = product;
            await _unitOfWork.SaveAsync();
            return productToUpdate;
        }
    }
}
