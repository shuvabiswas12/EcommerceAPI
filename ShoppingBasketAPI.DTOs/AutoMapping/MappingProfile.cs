using AutoMapper;
using ShoppingBasketAPI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasketAPI.DTOs.AutoMapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Create Mapping profile with ProductDTO and Product domain model.
            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.IsFeatured, opt => opt.MapFrom(src => src.FeaturedProduct != null))
                .ForMember(dest => dest.DiscountRate, opt => opt.MapFrom(src =>
                src.Discount != null
                ? (double)src.Price - (src.Discount.DiscountRate / 100) * (double)src.Price
                : 0.0))
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images));

            // Create Mapping profile with Image domain model and ImageResponseDTO.
            CreateMap<Image, ImageResponseDTO>();
        }
    }
}
