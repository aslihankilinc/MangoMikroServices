using Mango.Services.AuthApi.IContract;
using Mango.Services.AuthApi.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Mango.Services.AuthApi.Services
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtOptions _jwtOptions;
        //IOptions program.cs'de optio olarak tanımlandı
        public JwtTokenGenerator(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }
        //token oludturdugumuz yer
        //secret issuer ve audience appsettingsden gelecek
        public string GenerateToken(ApplicationUser applicationUser, IEnumerable<string> roles)
        {
          var tokenHandler=new JwtSecurityTokenHandler();
            //token key 
            //uygulma icerisine saklanacak
            var key=Encoding.ASCII.GetBytes(_jwtOptions.Secret);
            //key'i olusturacagımız objeler
            var claimsList = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Name,applicationUser.UserName),
                new Claim(JwtRegisteredClaimNames.Sub,applicationUser.Id)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _jwtOptions.Audience,
                Issuer = _jwtOptions.Issuer,
                Subject = new ClaimsIdentity(claimsList),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
