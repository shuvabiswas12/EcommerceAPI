using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShoppingBasketAPI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<ProductAvailability> ProductAvailabilities { get; set; }

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

            // Composite primary key
            builder.Entity<ShoppingCart>()
                .HasKey(x => new { x.ProductId, x.ApplicationUserId, x.CreatedAt });

            // Primary key
            builder.Entity<Discount>()
                .HasKey(x => new { x.ProductId });

            // Primary key
            builder.Entity<FeaturedProduct>()
                .HasKey(x => new { x.ProductId });

            // One to one relationship between Product and Discount
            builder.Entity<Product>()
                .HasOne(p => p.Discount)
                .WithOne(d => d.Product)
                .HasForeignKey<Discount>(d => d.ProductId);

            // One-to-one relationship between Product and FeaturedProduct
            builder.Entity<Product>()
                .HasOne(p => p.FeaturedProduct)
                .WithOne(f => f.Product)
                .HasForeignKey<FeaturedProduct>(f => f.ProductId);

            // One-to-one relationship between Product and ProductCategory
            builder.Entity<Product>()
                .HasOne(p => p.ProductCategory)
                .WithOne(pc => pc.Product)
                .HasForeignKey<ProductCategory>(pc => pc.ProductId);

            // Composite primary key for OrderDetail table
            builder.Entity<OrderDetail>()
                .HasKey(x => new { x.ProductId, x.OrderHeaderId });

            // One to many relationship
            // One orderHeader will be used in many orderDeatils row
            builder.Entity<OrderHeader>()
                .HasMany(oh => oh.OrderDetails)
                .WithOne(od => od.OrderHeader)
                .HasForeignKey(od => od.OrderHeaderId);

            // Defining primary key in ProductAvailability Table
            builder.Entity<ProductAvailability>()
                .HasKey(x => new { x.ProductId });

            // One to one relationship with Product and ProductAvailability tables.
            builder.Entity<Product>()
                .HasOne(p => p.ProductAvailability)
                .WithOne(pa => pa.Product)
                .HasForeignKey<ProductAvailability>(pa => pa.ProductId);
        }
    }
}