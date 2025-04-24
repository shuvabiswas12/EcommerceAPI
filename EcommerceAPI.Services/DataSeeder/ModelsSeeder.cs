using Microsoft.Extensions.DependencyInjection;
using EcommerceAPI.Data;
using EcommerceAPI.Data.UnitOfWork;
using EcommerceAPI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceAPI.Services.DataSeeder
{
    /// <summary>
    /// The task of this class is seed data for all Models except Roles and Users Model.
    /// </summary>
    public class ModelsSeeder
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IUnitOfWork _unitOfWork;

        public ModelsSeeder(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _unitOfWork = _serviceProvider.GetRequiredService<IUnitOfWork>();
        }

        public async Task SeedModelsAsync()
        {
            await this.SeedCategoryAsync();
            await this.SeedProductAsync();
        }

        private async Task SeedCategoryAsync()
        {
            var categoryData = await _unitOfWork.GenericRepository<Category>().GetAllAsync();
            if (!categoryData.Any())
            {
                await _unitOfWork.GenericRepository<Category>().AddRangeAsync(
                    new Category { Name = "Electronics" },
                    new Category { Name = "Fruits" },
                    new Category { Name = "Vegetables" },
                    new Category { Name = "Cloths" },
                    new Category { Name = "Households" }
                    );
                await _unitOfWork.SaveAsync();
            }
        }

        private async Task SeedProductAsync()
        {
            var productsData = await _unitOfWork.GenericRepository<Product>().GetAllAsync();
            if (!productsData.Any())
            {
                var product = new Product
                {
                    Name = "Samsung Galaxy A52",
                    Price = 32000,
                    CategoryId = _unitOfWork.GenericRepository<Category>().GetTAsync(predicate: c => c.Name == "Electronics").GetAwaiter().GetResult().Id,
                    Description = "Awesome screen, real smooth scrolling\r\n\r\nFeast your eyes on vibrant details with the FHD+ Super AMOLED " +
                    "display, reaching 800 nits for clarity even in bright daylight. Eye Comfort Shield\r\nlowers blue light," +
                    " and Real Smooth keeps the view smooth, whether you're gaming or scrolling. " +
                    "All on the\r\nexpansive 16.40cm (6.5\") Infinity-O Display.",
                };
                product.Discount = new Discount { DiscountRate = 10, DiscountEnabled = true, ProductId = product.Id };
                product.Images = new List<Image> {
                            new Image {ProductId = product.Id, ImageUrl = "https://www.shutterstock.com/image-photo/moscow-russia-march-30-2021-600nw-1947649531.jpg"},
                            new Image {ProductId = product.Id, ImageUrl = "https://www.shutterstock.com/image-photo/zaporozhye-ukraine-july-17-2022-600nw-2195912531.jpg"},
                            new Image {ProductId = product.Id, ImageUrl = "https://fdn.gsmarena.com/imgroot/reviews/21/samsung-galaxy-a52/lifestyle/-1024w2/gsmarena_001.jpg"}
                        };
                await _unitOfWork.GenericRepository<Product>().AddRangeAsync(product);
                await _unitOfWork.SaveAsync();
            }
        }
    }
}
