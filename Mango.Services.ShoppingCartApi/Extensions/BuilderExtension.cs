using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Mango.Services.ShoppingCartApi.Extensions
{
    public static class BuilderExtension
    {
        public static WebApplicationBuilder AppAuthetication(this WebApplicationBuilder builder)
        {
            //Authentication ve Authorization ayarları
            var tokenOptions = builder.Configuration.GetSection("ApiSettings:JwtOptions");
            var secret = tokenOptions.GetValue<string>("Secret");
            var issuer = tokenOptions.GetValue<string>("Issuer");
            var audience = tokenOptions.GetValue<string>("Audience");
            var key = Encoding.ASCII.GetBytes(secret);
            //kullanıcı kimliğini cookie değil, token
            //RequireHttpsMetadata = false    Geliştirme aşamasında HTTPS zorunluluğunu kapatır. (Prod’da true olmalı)
            //SaveToken = true	Doğrulanan token'ı HttpContext içine kaydeder (ileride erişmek istersen).
            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {   //Token’ı imzalayan anahtar (secret key) doğrulanacak
                    ValidateIssuerSigningKey = true,
                    //backend’de oluşturulan key
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience
                };
            });
            return builder;
        }

    }
}
