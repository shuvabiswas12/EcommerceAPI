using AutoMapper;
using EcommerceAPI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAPI.DTOs.AutoMapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryDTO>();
            CreateMap<Discount, DiscountDTO>();

            // Create Mapping profile with ProductDTO and Product domain model.
            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.Discount, opt => opt.MapFrom(src => src.Discount))
                .ForMember(dest => dest.CurrentAvailability, opt => opt.MapFrom(src => src.ProductAvailability.Availability))
                .ForMember(dest => dest.DiscountPrice, opt => opt.MapFrom(src =>
                src.Discount != null
                ? (double)src.Price - (src.Discount.DiscountRate / 100) * (double)src.Price
                : 0.0))

                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images.Select(img => img.ImageUrl)));

            CreateMap<ShoppingCart, CartDTO>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Product.Id))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price))
                .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src => src.Product.Images.Select(img => img.ImageUrl)));
        }
    }
}
