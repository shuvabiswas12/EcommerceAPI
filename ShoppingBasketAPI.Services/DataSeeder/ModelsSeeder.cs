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
    }
}
