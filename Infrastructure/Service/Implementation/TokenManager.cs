using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Domain.Service.Interface;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Service.Implementation
{
    public class TokenManager : ITokenManager
    {
        readonly AppSettings AppSettings;
        public TokenManager(IOptions<AppSettings> appSettings)
        {
            AppSettings = appSettings.Value;
        }

        public (byte[] salt, byte[] hash) CreatePasswordHash(string password)
        {
            using var hmac = new HMACSHA512();
            return (hmac.Key, hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }

        public string GenerateToken(string login)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(AppSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("Login", login) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature
                )
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public bool IsPasswordCorrect(string password, byte[] salt, byte[] hash)
        {
            using (var hmac = new HMACSHA512(salt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; ++i)
                {
                    if (computedHash[i] != hash[i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}