using ImpossibleCommits.Auth.Core.Options;
using ImpossibleCommits.Auth.Core.Services;

namespace ImpossibleCommits.Auth.Core.Tests
{
    public class AuthServiceTests
    {
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            var options = new ImpossibleAuthOptions
            {
                Jwt = new JwtOptions
                {
                    Secret = "this-is-a-very-strong-secret-key-123!",
                    Issuer = "ImpossibleCommits",
                    Audience = "ImpossibleCommits.Client",
                    AccessTokenExpiry = TimeSpan.FromMinutes(15),
                    RefreshTokenExpiry = TimeSpan.FromDays(7)
                }
            };

            _authService = new AuthService(options);
        }

        [Fact]
        public void HashPassword_ShouldReturnHashNotEqualToPlainText()
        {
            var plain = "MyPassword123!";
            var hash = _authService.HashPassword(plain);
            Assert.NotEqual(plain, hash);
        }

        [Fact]
        public void HashPassword_ShouldReturnDifferentHashEachTime()
        {
            var plain = "MyPassword123!";
            var hash1 = _authService.HashPassword(plain);
            var hash2 = _authService.HashPassword(plain);
            Assert.NotEqual(hash1, hash2);
        }

        [Fact]
        public void VerifyPassword_ShouldReturnTrue_WhenPasswordMatches()
        {
            var plain = "MyPassword123!";
            var hash = _authService.HashPassword(plain);
            var result = _authService.VerifyPassword(plain, hash);
            Assert.True(result);
        }

        [Fact]
        public void VerifyPassword_ShouldReturnFalse_WhenPasswordDoesNotMatch()
        {
            var hash = _authService.HashPassword("MyPassword123!");
            var result = _authService.VerifyPassword("WrongPassword!", hash);
            Assert.False(result);
        }
        [Fact]
        public void GenerateRefreshToken_ShouldReturnUniqueTokens()
        {
            var token1 = _authService.GenerateRefreshToken();
            var token2 = _authService.GenerateRefreshToken();
            Assert.NotEqual(token1, token2);
        }
    }
}
