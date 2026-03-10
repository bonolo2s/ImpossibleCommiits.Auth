using System.Security.Cryptography;
using System.Text;
using ImpossibleCommits.Auth.Core.Interfaces;
using ImpossibleCommits.Auth.Core.Options;

namespace ImpossibleCommits.Auth.Core.Services
{
    public class PasswordService : IPasswordService
    {
        private readonly PasswordOptions _options;

        public PasswordService(PasswordOptions options)
        {
            _options = options;
        }

        public bool MeetsStrengthRequirements(string password)
        {
            if (password.Length < _options.MinLength) return false;
            if (_options.RequireUppercase && !password.Any(char.IsUpper)) return false;
            if (_options.RequireLowercase && !password.Any(char.IsLower)) return false;
            if (_options.RequireDigit && !password.Any(char.IsDigit)) return false;
            if (_options.RequireSpecialChar && !password.Any(c => !char.IsLetterOrDigit(c))) return false;

            return true;
        }

        public bool IsExpired(DateTime lastChanged, int expiryDays)
        {
            return (DateTime.UtcNow - lastChanged).TotalDays >= expiryDays;
        }

        public bool IsReused(string recentHash, IEnumerable<string> previousHashes)
        {
            return previousHashes.Contains(recentHash);
        }

        public string GeneratePasswordResetToken(string userId)
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var raw = $"{userId}:{timestamp}:{Guid.NewGuid()}";
            var bytes = Encoding.UTF8.GetBytes(raw);
            using var sha = SHA256.Create();
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}