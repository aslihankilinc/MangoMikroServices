using AutoMapper;
using Mango.Services.ProductApi;
using Mango.Services.ProductApi.Data;
using Mango.Services.ProductApi.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SQLitePCL;

var builder = WebApplication.CreateBuilder(args);


Batteries.Init();
// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("MangoContext"));
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme, securityScheme: new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
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
builder.AppAuthetication();
app.UseAuthorization();
//default dosya yolu
app.UseStaticFiles();

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
        if (dbContext.Database.GetPendingMigrations().Count() > 0)
            dbContext.Database.Migrate();
    }
}