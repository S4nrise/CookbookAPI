using CookbookAPI.Abstractions;
using CookbookAPI.Configuration;
using CookbookAPI.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CookbookAPI.Services
{
    public class JwtTokenGenerator(IOptions<JwtOptions> options) : IJwtTokenGenerator
    {
        private readonly JwtOptions _jwtOptions = options.Value;
        public JwtToken GenerateJwtToken(User user)
        {
            SigningCredentials credentials = new(
                new SymmetricSecurityKey(Convert.FromBase64String(_jwtOptions.Secret)),
                SecurityAlgorithms.HmacSha256
                );

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.GivenName, user.Name),
                new Claim(JwtRegisteredClaimNames.FamilyName, user.Name),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var now = DateTime.UtcNow;

            var expiration = now.AddMinutes(5);

            JwtSecurityToken securityToken = new(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                expires: expiration,
                claims: claims,
                signingCredentials: credentials
                );

            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return new JwtToken
            {
                UserId = user.Id,
                Token = token,
                CreatedAt = now,
                ExpiresAt = expiration,
            };
        }
    }
}
