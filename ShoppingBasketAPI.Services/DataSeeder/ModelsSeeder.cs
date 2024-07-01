using Microsoft.Extensions.DependencyInjection;
using ShoppingBasketAPI.Data;
using ShoppingBasketAPI.Data.UnitOfWork;
using ShoppingBasketAPI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasketAPI.Services.DataSeeder
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
                await _unitOfWork.GenericRepository<Product>().AddRangeAsync(
                    new Product
                    {
                        Name = "Samsung Galaxy A52",
                        Price = 32000,
                        Discount = new Discount { DiscountRate = 10 },
                        ProductCategory = new ProductCategory { CategoryId = (await _unitOfWork.GenericRepository<Category>().GetTAsync(predicate: c => c.Name == "Electronics")).Id },
                        Description = "Awesome screen, real smooth scrolling\r\n\r\nFeast your eyes on vibrant details with the FHD+ Super AMOLED display, reaching 800 nits for clarity even in bright daylight. Eye Comfort Shield\r\nlowers blue light, and Real Smooth keeps the view smooth, whether you're gaming or scrolling. All on the\r\nexpansive 16.40cm (6.5\") Infinity-O Display.",
                        Images = new List<Image> {
                            new Image {ImageUrl = "https://www.shutterstock.com/image-photo/moscow-russia-march-30-2021-600nw-1947649531.jpg"},
                            new Image {ImageUrl = "https://www.shutterstock.com/image-photo/zaporozhye-ukraine-july-17-2022-600nw-2195912531.jpg"},
                            new Image {ImageUrl = "https://fdn.gsmarena.com/imgroot/reviews/21/samsung-galaxy-a52/lifestyle/-1024w2/gsmarena_001.jpg"}
                        },
                        FeaturedProduct = new FeaturedProduct { }
                    }
                );
                await _unitOfWork.SaveAsync();
            }
        }
    }
}
