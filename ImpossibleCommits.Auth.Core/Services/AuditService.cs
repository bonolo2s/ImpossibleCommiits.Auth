using ImpossibleCommits.Auth.Core.Interfaces;
using ImpossibleCommits.Auth.Core.Models;
using ImpossibleCommits.Auth.Core.Options;

namespace ImpossibleCommits.Auth.Core.Services
{
    public class AuditService : IAuditService
    {
        private readonly AuditOptions _options;

        public AuditService(AuditOptions options)
        {
            _options = options;
        }

        public AuditEvent LogLoginAttempt(string userId, string ipAddress, bool success)
        {
            if (!_options.Enabled || !_options.LogSuccessfulLogins)
                return Empty();

            return new AuditEvent
            {
                EventType = AuditEventType.LoginAttempt,
                UserId = userId,
                IpAddress = ipAddress,
                Success = success,
                Details = success ? "Login successful" : "Login failed"
            };
        }

        public AuditEvent LogFailedLogin(string userId, string ipAddress)
        {
            if (!_options.Enabled || !_options.LogFailedLogins)
                return Empty();

            return new AuditEvent
            {
                EventType = AuditEventType.FailedLogin,
                UserId = userId,
                IpAddress = ipAddress,
                Success = false,
                Details = "Failed login attempt"
            };
        }

        public AuditEvent LogTokenIssued(string userId, string tokenType)
        {
            if (!_options.Enabled || !_options.LogTokenUsage)
                return Empty();

            return new AuditEvent
            {
                EventType = AuditEventType.TokenIssued,
                UserId = userId,
                Success = true,
                Details = $"{tokenType} token issued"
            };
        }

        public AuditEvent LogTokenRefreshed(string userId)
        {
            if (!_options.Enabled || !_options.LogTokenUsage)
                return Empty();

            return new AuditEvent
            {
                EventType = AuditEventType.TokenRefreshed,
                UserId = userId,
                Success = true,
                Details = "Token refreshed"
            };
        }

        public AuditEvent LogTokenRevoked(string userId, string reason)
        {
            if (!_options.Enabled || !_options.LogTokenUsage)
                return Empty();

            return new AuditEvent
            {
                EventType = AuditEventType.TokenRevoked,
                UserId = userId,
                Success = true,
                Details = reason
            };
        }

        public AuditEvent LogPasswordChanged(string userId)
        {
            if (!_options.Enabled || !_options.LogPasswordChanges)
                return Empty();

            return new AuditEvent
            {
                EventType = AuditEventType.PasswordChanged,
                UserId = userId,
                Success = true,
                Details = "Password changed"
            };
        }

        public AuditEvent LogPasswordResetRequested(string userId)
        {
            if (!_options.Enabled || !_options.LogPasswordChanges)
                return Empty();

            return new AuditEvent
            {
                EventType = AuditEventType.PasswordResetRequested,
                UserId = userId,
                Success = true,
                Details = "Password reset requested"
            };
        }

        public AuditEvent LogSuspiciousActivity(string userId, string ipAddress, string details)
        {
            if (!_options.Enabled || !_options.LogSuspiciousActivity)
                return Empty();

            return new AuditEvent
            {
                EventType = AuditEventType.SuspiciousActivity,
                UserId = userId,
                IpAddress = ipAddress,
                Success = false,
                Details = details
            };
        }

        public AuditEvent LogLogout(string userId)
        {
            if (!_options.Enabled)
                return Empty();

            return new AuditEvent
            {
                EventType = AuditEventType.Logout,
                UserId = userId,
                Success = true,
                Details = "User logged out"
            };
        }

        private static AuditEvent Empty() => new AuditEvent();
    }
}