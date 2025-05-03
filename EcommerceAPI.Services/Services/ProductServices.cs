using AutoMapper;
using EcommerceAPI.Data.UnitOfWork;
using EcommerceAPI.Domain;
using EcommerceAPI.DTOs;
using EcommerceAPI.DTOs.GenericResponse;
using EcommerceAPI.Services.IServices;
using EcommerceAPI.Utilities.Exceptions;
using Microsoft.IdentityModel.Tokens;
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

        public async Task<String> CreateProduct(ProductCreateDTO productDto)
        {
            _isCategoryExist(productDto.CategoryId);
            var product = new Product
            {
                CategoryId = productDto.CategoryId,
                Name = productDto.Name.Trim(),
                Description = productDto.Description.Trim(),
                Price = productDto.Price,
            };
            product.ProductAvailability = new ProductAvailability
            {
                Availability = productDto.CurrentAvailability ?? 0,
                ProductId = product.Id,
            };

            product.Images = productDto.ImageUrls.Select(url => new Image { ImageUrl = url, ProductId = product.Id }).ToList();
            var createdProduct = await _unitOfWork.GenericRepository<Product>().AddAsync(product);
            await _unitOfWork.SaveAsync();
            return createdProduct.Id;
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
                throw new ApiException(System.Net.HttpStatusCode.NotFound, "The Product not found.");
            }
            await _unitOfWork.GenericRepository<Product>().DeleteAsync(productToDelete);
            await _unitOfWork.SaveAsync();
        }

        public async Task<GenericResponseDTO<ProductDTO>> GetAllProduct()
        {
            var productsResult = await _unitOfWork.GenericRepository<Product>().GetAllAsync(includeProperties: "Images, Discount, Category, ProductAvailability");
            var productsResponse = new GenericResponseDTO<ProductDTO>
            {
                Data = _mapper.Map<IEnumerable<ProductDTO>>(productsResult),
                Count = productsResult.Count()
            };
            return productsResponse;
        }

        public async Task<ProductDTO> GetProductById(object id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id), "Product ID must be provided.");
            }
            var productResult = await _unitOfWork.GenericRepository<Product>().GetTAsync(x => x.Id == id.ToString(), includeProperties: "Images, Discount, Category, ProductAvailability");
            if (productResult == null)
            {
                throw new ApiException(System.Net.HttpStatusCode.NotFound, "The Product not found.");
            }
            return _mapper.Map<ProductDTO>(productResult);
        }

        public async Task<Product> UpdateProduct(Object id, ProductUpdateDTO productDto)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id), "Product ID must be provided.");
            }

            var productToUpdate = await _unitOfWork.GenericRepository<Product>().GetTAsync(x => x.Id == id.ToString(), includeProperties: "Images, ProductAvailability, Discount");
            if (productToUpdate == null)
            {
                throw new ApiException(System.Net.HttpStatusCode.NotFound, "The Product not found.");
            }

            if (!string.IsNullOrEmpty(productDto!.Name)) productToUpdate.Name = productDto!.Name;
            if (!string.IsNullOrEmpty(productDto!.Description)) productToUpdate.Description = productDto!.Description;
            if (productDto!.Price > 0) productToUpdate.Price = (decimal)productDto!.Price;
            if (productDto!.ImageUrls!.Any()) productToUpdate.Images = productDto!.ImageUrls!.Select(u => new Image { ImageUrl = u, ProductId = productToUpdate.Id }).ToList();

            // Update Category
            if (!string.IsNullOrEmpty(productDto!.CategoryId))
            {
                _isCategoryExist(productDto.CategoryId);
                productToUpdate.CategoryId = productDto.CategoryId;
            }

            // Update Product Availability
            if (productDto.CurrentAvailability != null)
            {
                // Newly Create product availability
                if (productToUpdate.ProductAvailability != null)
                {
                    productToUpdate.ProductAvailability.LastAvailability = productToUpdate.ProductAvailability.Availability;
                    productToUpdate.ProductAvailability.Availability = productDto.CurrentAvailability ?? 0;
                    productToUpdate.ProductAvailability.ProductId = productToUpdate.Id;
                    productToUpdate.ProductAvailability.UpdatedAt = DateTime.Now;
                }

                // Update existing product availability
                else
                {
                    productToUpdate.ProductAvailability = new ProductAvailability
                    {
                        Availability = productDto.CurrentAvailability ?? 0,
                        ProductId = productToUpdate.Id,
                        UpdatedAt = DateTime.Now
                    };
                }
            }

            // Update Featured functionality
            if (productDto.IsFeatured > 1 || productDto.IsFeatured < 0)
                throw new ApiException(System.Net.HttpStatusCode.BadRequest, "SET 'isFeatured=1' if you want to product as featured. Otherwise SET 'isFeatured=0' to remove product from featured."); ;

            if (productDto.IsFeatured == 1) productToUpdate.IsFeatured = true;
            else if (productDto.IsFeatured == 0) productToUpdate.IsFeatured = false;

            productToUpdate.Discount = this._setDiscount(productDto, productToUpdate.Discount, productToUpdate.Id);
            await _unitOfWork.SaveAsync();
            return productToUpdate;
        }

        private Discount? _setDiscount(ProductUpdateDTO productDto, Discount? discount, string productId)
        {
            // If discount exists in DB
            if (discount != null)
            {
                discount.DiscountRate = productDto.DiscountRate > 0.0 ? productDto.DiscountRate : discount.DiscountRate;

                // Update Discount Status
                if (productDto.DiscountEnabled > 1 || productDto.DiscountEnabled < 0)
                    throw new ApiException(System.Net.HttpStatusCode.BadGateway,
                    "SET 'discountEnabled=1' if you want to resume product discount. Otherwise SET 'discountEnabled=0' to pause product discount.");

                if (productDto.DiscountEnabled == 1) discount.DiscountEnabled = true;
                else if (productDto.DiscountEnabled == 0) discount.DiscountEnabled = false;


                if (productDto?.DiscountStartTimestamp != null)
                {
                    long dt = productDto.DiscountStartTimestamp ?? DateTimeOffset.Now.ToUnixTimeSeconds();
                    discount.DiscountStartAt = DateTimeOffset.FromUnixTimeSeconds(dt).DateTime;
                }

                if (productDto?.DiscountEndTimestamp != null)
                {
                    long dt = productDto.DiscountEndTimestamp ?? 0;
                    discount.DiscountEndAt = DateTimeOffset.FromUnixTimeSeconds(dt).DateTime;
                }
            }
            // If discount does not exist in DB, create new if valid data is present
            else
            {
                if (productDto.DiscountRate <= 0.0 && productDto.DiscountEnabled == 0)
                    return null; // No discount to set

                discount = new Discount
                {
                    DiscountRate = productDto.DiscountRate > 0.0 ? productDto.DiscountRate : 0.0,
                    DiscountEnabled = true,
                    DiscountStartAt = productDto.DiscountStartTimestamp != null
                        ? DateTimeOffset.FromUnixTimeSeconds(productDto.DiscountStartTimestamp.Value).DateTime
                        : DateTime.UtcNow,
                    DiscountEndAt = productDto.DiscountEndTimestamp != null
                        ? DateTimeOffset.FromUnixTimeSeconds(productDto.DiscountEndTimestamp.Value).DateTime
                        : (DateTime?)null,
                    ProductId = productId
                };
            }
            return discount;
        }


        private Boolean _isCategoryExist(object id)
        {
            try
            {
                Category category = _unitOfWork.GenericRepository<Category>().GetTAsync(c => c.Id == id.ToString()).GetAwaiter().GetResult();
                return category is not null ? true : throw new Exception();
            }
            catch (Exception ex)
            {
                throw new ApiException(System.Net.HttpStatusCode.NotFound, "The Category is not available.");
            }
        }
    }
}
