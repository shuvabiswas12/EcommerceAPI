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
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<FeaturedProduct> FeaturedProducts { get; set; }

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

            // Composite primary key
            builder.Entity<ProductCategory>()
                .HasKey(x => new { x.ProductId, x.CategoryId });

            // Primary key
            builder.Entity<Discount>()
                .HasKey(x => new { x.ProductId });

            // Primary key
            builder.Entity<FeaturedProduct>()
                .HasKey(x => new { x.ProductId });
        }
    }
}