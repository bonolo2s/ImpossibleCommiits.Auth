using ImpossibleCommits.Auth.Core.Models;

namespace ImpossibleCommits.Auth.Core.Interfaces
{
    public interface IAuditService
    {
        AuditEvent LogLoginAttempt(string userId, string ipAddress, bool success);
        AuditEvent LogFailedLogin(string userId, string ipAddress);
        AuditEvent LogTokenIssued(string userId, string tokenType);
        AuditEvent LogTokenRefreshed(string userId);
        AuditEvent LogTokenRevoked(string userId, string reason);
        AuditEvent LogPasswordChanged(string userId);
        AuditEvent LogPasswordResetRequested(string userId);
        AuditEvent LogSuspiciousActivity(string userId, string ipAddress, string details);
        AuditEvent LogLogout(string userId);
    }
}