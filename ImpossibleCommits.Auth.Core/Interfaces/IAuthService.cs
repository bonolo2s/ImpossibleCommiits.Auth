using ImpossibleCommits.Auth.Core.Models;
using System.Security.Claims;

namespace ImpossibleCommits.Auth.Core.Interfaces
{
    public interface IAuthService
    {
        string HashPassword(string plainText);
        bool VerifyPassword(string plainText, string hash);
        AuthToken GenerateToken(string userId, IEnumerable<Claim> claims);
        IEnumerable<Claim>? ValidateToken(string token);
        string GenerateRefreshToken();
        bool ValidateRefreshToken(string token);
        bool RevokeToken(string token);
        void Logout(string userId);
    }
}