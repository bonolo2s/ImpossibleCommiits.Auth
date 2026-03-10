namespace ImpossibleCommits.Auth.Core.Interfaces
{
    public interface IPasswordService
    {
        bool MeetsStrengthRequirements(string password);
        bool IsExpired(DateTime lastChanged, int expiryDays);
        bool IsReused(string newHash, IEnumerable<string> previousHashes);
        string GeneratePasswordResetToken(string userId); 
    }
}