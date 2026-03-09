using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ImpossibleCommits.Auth.Core.Interfaces;
using ImpossibleCommits.Auth.Core.Models;
using Microsoft.IdentityModel.Tokens;

namespace ImpossibleCommits.Auth.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly ImpossibleAuthOptions _options;

        public AuthService(ImpossibleAuthOptions options)
        {
            _options = options;
        }

        public string HashPassword(string plainText)
        {
            return BCrypt.Net.BCrypt.HashPassword(plainText);
        }

        public bool VerifyPassword(string plainText, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(plainText, hash);
        }

        public AuthToken GenerateToken(string userId, IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Jwt.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.UtcNow.Add(_options.Jwt.AccessTokenExpiry);

            var allClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            allClaims.AddRange(claims);

            var token = new JwtSecurityToken(
                issuer: _options.Jwt.Issuer,
                audience: _options.Jwt.Audience,
                claims: allClaims,
                expires: expiry,
                signingCredentials: creds
            );

            return new AuthToken
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = GenerateRefreshToken(),
                ExpiresAt = expiry
            };
        }

        public IEnumerable<Claim>? ValidateToken(string token)
        {
            try
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Jwt.Secret));
                var handler = new JwtSecurityTokenHandler();

                var principal = handler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = true,
                    ValidIssuer = _options.Jwt.Issuer,
                    ValidateAudience = true,
                    ValidAudience = _options.Jwt.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out _);

                return principal.Claims;
            }
            catch
            {
                return null;
            }
        }

        public string GenerateRefreshToken()
        {
            var bytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }
    }
}