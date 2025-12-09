using AutoMapper;
using Mango.MessageBus.IContract;
using Mango.MessageBus.Services;
using Mango.Services.CartApi;
using Mango.Services.CartApi.Data;
using Mango.Services.CartApi.Extensions;
using Mango.Services.CartApi.IContract;
using Mango.Services.CartApi.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SQLitePCL;

var builder = WebApplication.CreateBuilder(args);

Batteries.Init();

// DBContext
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("MangoContext"));
});
// Add services to the container.

builder.Services.AddControllers();
//ProductService yapilandirma
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IMessageBus, MessageBus>();
//Handler ekleme
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<AuthHttpClientHandler>();

builder.Services.AddHttpClient("Product",u=>u.BaseAddress=
                               new Uri(builder.Configuration["ServiceUrls:ProductAPI"]))
                               .AddHttpMessageHandler<AuthHttpClientHandler>();

//CouponService yapilandirma
builder.Services.AddScoped<ICouponService, CouponService>();
builder.Services.AddHttpClient("Coupon", u => u.BaseAddress = 
                                new Uri(builder.Configuration["ServiceUrls:CouponAPI"]))
                                 .AddHttpMessageHandler<AuthHttpClientHandler>(); 
 
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme, securityScheme: new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter the Bearer token like: Bearer <JWT>",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference= new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id=JwtBearerDefaults.AuthenticationScheme
                }
            }, new string[]{}
        }
    });
});

// AutoMapper
IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddSwaggerGen();

// JWT Authentication Extension
builder.AppAuthetication();

var app = builder.Build();

// Development ortamýnda Swagger aktif
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//  Authentication önce olmalý
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
ApplyMigrations();
app.Run();
/// <summary>
/// Varolan migrasyonlari uygular
/// </summary>
void ApplyMigrations()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        if (dbContext.Database.GetPendingMigrations().Any())
            dbContext.Database.Migrate();
    }
}
