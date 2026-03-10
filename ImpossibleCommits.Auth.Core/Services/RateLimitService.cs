using ImpossibleCommits.Auth.Core.Interfaces;
using ImpossibleCommits.Auth.Core.Options;

namespace ImpossibleCommits.Auth.Core.Services
{
    public class RateLimitService : IRateLimitService
    {
        private readonly RateLimitOptions _options;

        public RateLimitService(RateLimitOptions options)
        {
            _options = options;
        }

        public bool IsAllowed(string action, int attemptCount, DateTime windowStart)
        {
            if (DateTime.UtcNow - windowStart > _options.Window)
                return true;

            return action switch
            {
                "login" => attemptCount < _options.MaxLoginAttempts,
                "password_reset" => attemptCount < _options.MaxPasswordResetAttempts,
                "token_refresh" => attemptCount < _options.MaxTokenRefreshAttempts,
                _ => true
            };
        }

        public void Reset(string userId, string action)
        {
            // Stateless — caller will reset in their DB
        }
    }
}