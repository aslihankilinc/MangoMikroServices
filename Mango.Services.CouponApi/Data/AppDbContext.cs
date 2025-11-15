using Mango.Services.CouponApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.CouponApi.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Coupon> Coupons { get; set; }
    }
}

/*
protected readonly IConfiguration configuration;
private string _connectionStringOrName;

public AppDbContext(string connectionStringOrName)
{
    this._connectionStringOrName = connectionStringOrName;
    var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
    configuration = builder;
}
public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions) : base(dbContextOptions) { }
protected override void OnConfiguring(DbContextOptionsBuilder options)
{
    // connect to sqlite database
    string connectionString = string.Empty;
    connectionString = configuration.GetConnectionString("DbContext");
    var result = options.UseSqlite(connectionString);
}
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);
}*/