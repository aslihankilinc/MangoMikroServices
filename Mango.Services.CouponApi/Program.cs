using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using Mango.Services.CouponApi.Data;
using AutoMapper;
using Mango.Services.CouponApi;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
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

//Authentication ve Authorization ayarlarý
var secret = builder.Configuration.GetValue<string>("ApiSettings:Secret");
var issuer = builder.Configuration.GetValue<string>("ApiSettings:Issuer");
var audience = builder.Configuration.GetValue<string>("ApiSettings:Audience");
var key = Encoding.ASCII.GetBytes(secret);


//kullanýcý kimliðini cookie deðil, token
//RequireHttpsMetadata = false    Geliþtirme aþamasýnda HTTPS zorunluluðunu kapatýr. (Prod’da true olmalý)
//SaveToken = true	Doðrulanan token'ý HttpContext içine kaydeder (ileride eriþmek istersen).
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {   //Token’ý imzalayan anahtar (secret key) doðrulanacak
        ValidateIssuerSigningKey = true,
        //backend’de oluþturulan key
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = issuer,
        ValidAudience = audience
    };
}
    );



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
        if (dbContext.Database.GetPendingMigrations().Count() > 0)
            dbContext.Database.Migrate();
    }
}