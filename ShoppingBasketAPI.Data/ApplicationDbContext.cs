using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShoppingBasketAPI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasketAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // One to many relationship
            // One product has many images
            builder.Entity<Product>()
                .HasMany(p => p.Images)
                .WithOne(i => i.Product)
                .HasForeignKey(i => i.ProductId);

            // Composite primary key
            builder.Entity<Image>()
                .HasKey(x => new { x.ProductId, x.ImageUrl });

            // Data seeding - Category model
            builder.Entity<Category>().HasData(
                new Category { Id = Guid.NewGuid().ToString(), Name = "Electronics", CreatedDate = DateTime.Now },
                new Category { Id = Guid.NewGuid().ToString(), Name = "Cloths", CreatedDate = DateTime.Now },
                new Category { Id = Guid.NewGuid().ToString(), Name = "Fruits", CreatedDate = DateTime.Now },
                new Category { Id = Guid.NewGuid().ToString(), Name = "Vegetables", CreatedDate = DateTime.Now },
                new Category { Id = Guid.NewGuid().ToString(), Name = "Households", CreatedDate = DateTime.Now }
                );

            // Data seeding - Product model
            builder.Entity<Product>().HasData(
                new Product
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Samsung Galaxy A52 4g",
                    Price = 25000,
                    Description = "***Galaxy A52 is rated as IP67. " +
                    "Based on test conditions for submersion in up to 1 meter of freshwater for up to 30 minutes. " +
                    "Not advised for beach, pool use and soapy water. In case you spill liquids containing sugar on the phone, " +
                    "please rinse the device in clean, stagnant water while clicking keys. Safe against low water pressure only. " +
                    "High water pressure such as running tap water or shower may damage the device."
                }
            );

            // Data seeding - Image model
            builder.Entity<Image>().HasData(
                new Image { ProductId = "d72e6d76-ffb8-4b6a-b2b7-5945f84992d9", ImageUrl = "https://technave.com/data/files/mall/article/202103171440419975.jpg" }
            );
        }
    }
}
