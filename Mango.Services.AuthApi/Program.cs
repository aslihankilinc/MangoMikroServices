using AutoMapper;
using Mango.Services.AuthApi.Data;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using Microsoft.AspNetCore.Identity;
using Mango.Services.AuthApi.Models;
using Mango.Services.AuthApi.IContract;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<IAuthService,IAuthService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
Batteries.Init();
// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("MangoContext"));
});
//Identity DB baglami yapýlandýrma
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("ApiSettings:JwtOptions"));
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

///Identity,once giris sonra yetkilendirme
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
ApplyMigrations();
app.Run();
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