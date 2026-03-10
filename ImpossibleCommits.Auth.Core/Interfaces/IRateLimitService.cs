namespace ImpossibleCommits.Auth.Core.Interfaces
{
    public interface IRateLimitService
    {
        bool IsAllowed(string action, int attemptCount, DateTime windowStart); //pass options ill enforce for you
        void Reset(string userId, string action);
    }
}