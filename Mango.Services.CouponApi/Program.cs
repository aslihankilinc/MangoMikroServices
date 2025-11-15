using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using Mango.Services.CouponApi.Data;
using AutoMapper;
using Mango.Services.CouponApi;
var builder = WebApplication.CreateBuilder(args);
Batteries.Init();
// Add services to the container.
builder.Services.AddDbContext<Mango.Services.CouponApi.Data.AppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("MangoContext"));
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//AutoMapper Config Islemleri

IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);
//AutoMapper Config Islemleri

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
ApplyMigrations();

app.Run();

/// <summary>Varolan migrasyonlari uygular</summary>
void ApplyMigrations()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        ///<summary>bekleyen migration islemi varsa yap yoksa hata fýrlatýr</summary>
        if(dbContext.Database.GetPendingMigrations().Count()>0)
        dbContext.Database.Migrate();
    }
}