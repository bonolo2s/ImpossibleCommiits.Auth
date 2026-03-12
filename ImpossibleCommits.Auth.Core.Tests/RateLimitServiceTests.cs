using ImpossibleCommits.Auth.Core.Options;
using ImpossibleCommits.Auth.Core.Services;

namespace ImpossibleCommits.Auth.Core.Tests
{
    public class RateLimitServiceTests
    {
        private readonly RateLimitService _rateLimitService;

        public RateLimitServiceTests()
        {
            var options = new RateLimitOptions
            {
                MaxLoginAttempts = 5,
                MaxPasswordResetAttempts = 3,
                MaxTokenRefreshAttempts = 10,
                Window = TimeSpan.FromMinutes(10)
            };

            _rateLimitService = new RateLimitService(options);
        }

        // IsAllowed — window expired

        [Fact]
        public void IsAllowed_ShouldReturnTrue_WhenWindowHasExpired()
        {
            var windowStart = DateTime.UtcNow.AddMinutes(-11);
            var result = _rateLimitService.IsAllowed("login", 10, windowStart);
            Assert.True(result);
        }

        // IsAllowed — login

        [Fact]
        public void IsAllowed_ShouldReturnTrue_WhenLoginAttemptsAreBelowMax()
        {
            var result = _rateLimitService.IsAllowed("login", 4, DateTime.UtcNow);
            Assert.True(result);
        }

        [Fact]
        public void IsAllowed_ShouldReturnFalse_WhenLoginAttemptsReachMax()
        {
            var result = _rateLimitService.IsAllowed("login", 5, DateTime.UtcNow);
            Assert.False(result);
        }

        [Fact]
        public void IsAllowed_ShouldReturnFalse_WhenLoginAttemptsExceedMax()
        {
            var result = _rateLimitService.IsAllowed("login", 9, DateTime.UtcNow);
            Assert.False(result);
        }

        // IsAllowed — password_reset

        [Fact]
        public void IsAllowed_ShouldReturnTrue_WhenPasswordResetAttemptsAreBelowMax()
        {
            var result = _rateLimitService.IsAllowed("password_reset", 2, DateTime.UtcNow);
            Assert.True(result);
        }

        [Fact]
        public void IsAllowed_ShouldReturnFalse_WhenPasswordResetAttemptsReachMax()
        {
            var result = _rateLimitService.IsAllowed("password_reset", 3, DateTime.UtcNow);
            Assert.False(result);
        }

        // IsAllowed — token_refresh

        [Fact]
        public void IsAllowed_ShouldReturnTrue_WhenTokenRefreshAttemptsAreBelowMax()
        {
            var result = _rateLimitService.IsAllowed("token_refresh", 9, DateTime.UtcNow);
            Assert.True(result);
        }

        [Fact]
        public void IsAllowed_ShouldReturnFalse_WhenTokenRefreshAttemptsReachMax()
        {
            var result = _rateLimitService.IsAllowed("token_refresh", 10, DateTime.UtcNow);
            Assert.False(result);
        }

        // IsAllowed — unknown action

        [Fact]
        public void IsAllowed_ShouldReturnTrue_WhenActionIsUnknown()
        {
            var result = _rateLimitService.IsAllowed("unknown_action", 99, DateTime.UtcNow);
            Assert.True(result);
        }
    }
}