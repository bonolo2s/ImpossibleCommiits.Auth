using ImpossibleCommits.Auth.Core.Options;
using ImpossibleCommits.Auth.Core.Services;

namespace ImpossibleCommits.Auth.Core.Tests
{
    public class PasswordServiceTests
    {
        private readonly PasswordService _passwordService;

        public PasswordServiceTests()
        {
            var options = new PasswordOptions
            {
                MinLength = 8,
                RequireUppercase = true,
                RequireLowercase = true,
                RequireDigit = true,
                RequireSpecialChar = true
            };

            _passwordService = new PasswordService(options);
        }

        // MeetsStrengthRequirements

        [Fact]
        public void MeetsStrengthRequirements_ShouldReturnTrue_WhenPasswordIsValid()
        {
            var result = _passwordService.MeetsStrengthRequirements("Abcdef1!");
            Assert.True(result);
        }

        [Fact]
        public void MeetsStrengthRequirements_ShouldReturnFalse_WhenPasswordIsTooShort()
        {
            var result = _passwordService.MeetsStrengthRequirements("Ab1!");
            Assert.False(result);
        }

        [Fact]
        public void MeetsStrengthRequirements_ShouldReturnFalse_WhenMissingUppercase()
        {
            var result = _passwordService.MeetsStrengthRequirements("abcdef1!");
            Assert.False(result);
        }

        [Fact]
        public void MeetsStrengthRequirements_ShouldReturnFalse_WhenMissingLowercase()
        {
            var result = _passwordService.MeetsStrengthRequirements("ABCDEF1!");
            Assert.False(result);
        }

        [Fact]
        public void MeetsStrengthRequirements_ShouldReturnFalse_WhenMissingDigit()
        {
            var result = _passwordService.MeetsStrengthRequirements("Abcdefg!");
            Assert.False(result);
        }

        [Fact]
        public void MeetsStrengthRequirements_ShouldReturnFalse_WhenMissingSpecialChar()
        {
            var result = _passwordService.MeetsStrengthRequirements("Abcdef12");
            Assert.False(result);
        }

        // IsExpired

        [Fact]
        public void IsExpired_ShouldReturnTrue_WhenPasswordIsOlderThanExpiryDays()
        {
            var lastChanged = DateTime.UtcNow.AddDays(-91);
            var result = _passwordService.IsExpired(lastChanged, 90);
            Assert.True(result);
        }

        [Fact]
        public void IsExpired_ShouldReturnTrue_WhenPasswordIsExactlyAtExpiryDays()
        {
            var lastChanged = DateTime.UtcNow.AddDays(-90);
            var result = _passwordService.IsExpired(lastChanged, 90);
            Assert.True(result);
        }

        [Fact]
        public void IsExpired_ShouldReturnFalse_WhenPasswordIsWithinExpiryDays()
        {
            var lastChanged = DateTime.UtcNow.AddDays(-30);
            var result = _passwordService.IsExpired(lastChanged, 90);
            Assert.False(result);
        }

        [Fact]
        public void IsExpired_ShouldReturnFalse_WhenPasswordWasJustChanged()
        {
            var result = _passwordService.IsExpired(DateTime.UtcNow, 90);
            Assert.False(result);
        }

        // IsReused

        [Fact]
        public void IsReused_ShouldReturnTrue_WhenHashExistsInHistory()
        {
            var hash = "abc123hash";
            var result = _passwordService.IsReused(hash, new[] { "old1", hash, "old2" });
            Assert.True(result);
        }

        [Fact]
        public void IsReused_ShouldReturnFalse_WhenHashDoesNotExistInHistory()
        {
            var result = _passwordService.IsReused("newHash", new[] { "old1", "old2" });
            Assert.False(result);
        }

        [Fact]
        public void IsReused_ShouldReturnFalse_WhenHistoryIsEmpty()
        {
            var result = _passwordService.IsReused("anyHash", Array.Empty<string>());
            Assert.False(result);
        }

        // GeneratePasswordResetToken

        [Fact]
        public void GeneratePasswordResetToken_ShouldReturnNonEmptyToken()
        {
            var token = _passwordService.GeneratePasswordResetToken("user-1");
            Assert.NotEmpty(token);
        }

        [Fact]
        public void GeneratePasswordResetToken_ShouldReturnUniqueTokens()
        {
            var token1 = _passwordService.GeneratePasswordResetToken("user-1");
            var token2 = _passwordService.GeneratePasswordResetToken("user-1");
            Assert.NotEqual(token1, token2);
        }

        [Fact]
        public void GeneratePasswordResetToken_ShouldReturnDifferentTokens_ForDifferentUsers()
        {
            var token1 = _passwordService.GeneratePasswordResetToken("user-1");
            var token2 = _passwordService.GeneratePasswordResetToken("user-2");
            Assert.NotEqual(token1, token2);
        }

        [Fact]
        public void GeneratePasswordResetToken_ShouldReturnValidBase64String()
        {
            var token = _passwordService.GeneratePasswordResetToken("user-1");
            var ex = Record.Exception(() => Convert.FromBase64String(token));
            Assert.Null(ex);
        }
    }
}