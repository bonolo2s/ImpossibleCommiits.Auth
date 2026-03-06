namespace ImpossibleCommits.Auth.Core.Models
{
    public enum AuditEventType
    {
        LoginAttempt,
        FailedLogin,
        TokenIssued,
        TokenRefreshed,
        TokenRevoked,
        PasswordChanged,
        PasswordResetRequested,
        SuspiciousActivity,
        Logout
    }
}