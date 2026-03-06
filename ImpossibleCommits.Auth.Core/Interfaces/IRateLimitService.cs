namespace ImpossibleCommits.Auth.Core.Interfaces
{
    public interface IRateLimitService
    {
        bool IsAllowed(string userId, string action);
        void RecordAttempt(string userId, string action);
        int GetRemainingAttempts(string userId, string action);
        void Reset(string userId, string action);
    }
}