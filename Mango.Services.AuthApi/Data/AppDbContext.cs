
using Mango.Services.AuthApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.AuthApi.Data
{
    public class AppDbContext:IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole
            {
                Id = "477e7c33-d4b7-4466-9c35-1c9c8a0b656a",
                ConcurrencyStamp=null,
                Name = "CUSTOMER",
                NormalizedName = "CUSTOMER",
            });
             modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole
            {
           
                Id= "ff1db8e8-7c65-471f-861f-7bf5c1f75fe2",
                ConcurrencyStamp=null,
                Name = "ADMIN",
                NormalizedName = "ADMIN",
            }
        );
        }

    }
}

