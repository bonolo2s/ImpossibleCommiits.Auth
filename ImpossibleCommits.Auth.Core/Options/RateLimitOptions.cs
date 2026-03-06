namespace ImpossibleCommits.Auth.Core.Options
{
    public class RateLimitOptions
    {
        public int MaxLoginAttempts { get; set; } = 5;
        public int MaxPasswordResetAttempts { get; set; } = 3;
        public int MaxTokenRefreshAttempts { get; set; } = 10;
        public TimeSpan Window { get; set; } = TimeSpan.FromMinutes(10);
    }
}