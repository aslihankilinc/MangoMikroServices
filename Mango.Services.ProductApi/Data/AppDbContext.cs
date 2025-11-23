
using Mango.Services.ProductApi.Models;
using Microsoft.EntityFrameworkCore;
namespace Mango.Services.ProductApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductId = 1,
                Name = "Ekmek",
                Price = 15,
                Description = "Ekmek",
                ImageUrl = "https://placehold.co/603x403",
                CategoryName = "Appetizer"
            });
            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductId = 2,
                Name = "Kepekli Ekmek",
                Price = 13.99,
                Description = "Kepekli Ekmek",
                ImageUrl = "https://placehold.co/602x402",
                CategoryName = "Appetizer"
            });
            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductId = 3,
                Name = "Çavdar Ekmeği",
                Price = 10.99,
                Description = "Çavdar Ekmeği",
                ImageUrl = "https://placehold.co/601x401",
                CategoryName = "Dessert"
            });
            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductId = 4,
                Name = "Simit",
                Price = 15,
                Description = "Simit",
                ImageUrl = "https://placehold.co/600x400",
                CategoryName = "Entree"
            });
        }
    }
}