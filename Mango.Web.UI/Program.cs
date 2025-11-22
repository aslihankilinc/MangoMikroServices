using Mango.Web.UI.IContract;
using Mango.Web.UI.Services;
using Mango.Web.UI.Utility;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//AddHttpContextAccessor
//TokenService için gerekli
//Baþlýklar, çerezler, sorgu parametreleri ve kullanýcý talepleri gibi HTTP istek
//ve yanýtýnýn çeþitli yönlerine eriþmenizi saðlar
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();

//Servis URL'leri appsettings.json dosyasýndan okunuyor7

builder.Services.AddHttpClient<ICouponService, CouponService>();
builder.Services.AddScoped<IBaseService, BaseService>();
builder.Services.AddScoped<ICouponService, CouponService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
//appsettings servin kostugu port url'li
StaticBase.CouponApiBase = builder.Configuration["ServiceUrls:CouponAPI"];
StaticBase.AuthApiBase = builder.Configuration["ServiceUrls:AuthAPI"];
//Uygulama kimlik doðrulama için cookie (çerez) yöntemini kullanacak
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromHours(10);
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/AccessDenied";
    });


//

//servisler önce eklenmeli
var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//login ayarlarý
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
