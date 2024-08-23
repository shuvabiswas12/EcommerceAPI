using EcommerceAPI.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAPI.Services.IServices
{
    public interface IFeaturedProductServices
    {
        public Task AddProductAsFeatured(FeaturedProductRequestDTO featuredProductRequestDTO);
        public Task RemoveProductFromFeatured(FeaturedProductRequestDTO featuredProductRequestDTO);
    }
}
