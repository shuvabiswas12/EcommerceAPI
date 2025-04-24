using AutoMapper;
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
    public class ProductServices : IProductServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Product> CreateProduct(ProductCreateDTO productDto)
        {
            var product = new Product
            {
                CategoryId = productDto.CategoryId,
                Name = productDto.Name.Trim(),
                Description = productDto.Description.Trim(),
                Price = productDto.Price,
            };
            product.Images = productDto.ImageUrls.Select(url => new Image { ImageUrl = url, ProductId = product.Id }).ToList();

            var createdProduct = await _unitOfWork.GenericRepository<Product>().AddAsync(product);
            await _unitOfWork.SaveAsync();
            return createdProduct;
        }

        public async Task DeleteProduct(object id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id), "Product ID must be provided.");
            }
            var productToDelete = await _unitOfWork.GenericRepository<Product>().GetTAsync(x => x.Id == id.ToString(), includeProperties: "Images");
            if (productToDelete == null)
            {
                throw new NotFoundException("Product not found.");
            }
            await _unitOfWork.GenericRepository<Product>().DeleteAsync(productToDelete);
            await _unitOfWork.SaveAsync();
        }

        public async Task<GenericResponseDTO<ProductDTO>> GetAllProduct()
        {
            var productsResult = await _unitOfWork.GenericRepository<Product>().GetAllAsync(includeProperties: "Images, Discount, ProductCategory, FeaturedProduct");
            var productsResponse = new GenericResponseDTO<ProductDTO>
            {
                Data = _mapper.Map<IEnumerable<ProductDTO>>(productsResult),
                Count = productsResult.Count()
            };
            return productsResponse;
        }

        public async Task<Product> GetProductById(object id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id), "Product ID must be provided.");
            }
            var productResult = await _unitOfWork.GenericRepository<Product>().GetTAsync(x => x.Id == id.ToString(), includeProperties: "Images");
            if (productResult == null)
            {
                throw new NotFoundException(message: "Product not found.");
            }
            return productResult;
        }

        public async Task<Product> UpdateProduct(Object id, ProductUpdateDTO productDto)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id), "Product ID must be provided.");
            }

            var productToUpdate = await _unitOfWork.GenericRepository<Product>().GetTAsync(x => x.Id == id.ToString(), includeProperties: "Images");
            if (productToUpdate == null)
            {
                throw new NotFoundException(message: "Product not found.");
            }
            if (!string.IsNullOrEmpty(productDto!.Name)) productToUpdate.Name = productDto!.Name;
            if (!string.IsNullOrEmpty(productDto!.Description)) productToUpdate.Description = productDto!.Description;
            if (productDto!.Price > 0) productToUpdate.Price = (decimal)productDto!.Price;
            if (productDto!.ImageUrls!.Any()) productToUpdate.Images = productDto!.ImageUrls!.Select(u => new Image { ImageUrl = u, ProductId = productToUpdate.Id }).ToList();

            await _unitOfWork.SaveAsync();
            return productToUpdate;
        }
    }
}
