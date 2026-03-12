using ImpossibleCommits.Auth.Core.Models;
using ImpossibleCommits.Auth.Core.Options;
using ImpossibleCommits.Auth.Core.Services;

namespace ImpossibleCommits.Auth.Core.Tests
{
    public class AuditServiceTests
    {
        private readonly AuditService _auditService;

        public AuditServiceTests()
        {
            var options = new AuditOptions
            {
                Enabled = true,// parent?
                LogSuccessfulLogins = true,
                LogFailedLogins = true,
                LogTokenUsage = true,
                LogPasswordChanges = true,
                LogSuspiciousActivity = true
            };

            _auditService = new AuditService(options);
        }

        // LogLoginAttempt

        [Fact]
        public void LogLoginAttempt_ShouldReturnEvent_WhenSuccessful()
        {
            var result = _auditService.LogLoginAttempt("user-1", "127.0.0.1", true);
            Assert.Equal(AuditEventType.LoginAttempt, result.EventType);
            Assert.True(result.Success);
        }

        [Fact]
        public void LogLoginAttempt_ShouldReturnEvent_WhenFailed()
        {
            var result = _auditService.LogLoginAttempt("user-1", "127.0.0.1", false);
            Assert.Equal(AuditEventType.LoginAttempt, result.EventType);
            Assert.False(result.Success);
        }

        [Fact]
        public void LogLoginAttempt_ShouldReturnEmptyEvent_WhenAuditingIsDisabled()
        {
            var svc = new AuditService(new AuditOptions { Enabled = false });
            var result = svc.LogLoginAttempt("user-1", "127.0.0.1", true);
            Assert.Null(result.UserId);
        }

        [Fact]
        public void LogLoginAttempt_ShouldReturnEmpty_WhenLogSuccessfulLoginsIsFalse()
        {
            var svc = new AuditService(new AuditOptions { LogSuccessfulLogins = false });
            var result = svc.LogLoginAttempt("user-1", "127.0.0.1", true);
            Assert.Equal(new AuditEvent().EventType, result.EventType);
        }

        // LogFailedLogin

        [Fact]
        public void LogFailedLogin_ShouldReturnEvent_WhenEnabled()
        {
            var result = _auditService.LogFailedLogin("user-1", "127.0.0.1");
            Assert.Equal(AuditEventType.FailedLogin, result.EventType);
            Assert.False(result.Success);
        }

        [Fact]
        public void LogFailedLogin_ShouldReturnEmptyEvent_WhenLogFailedLoginsIsDisabled()
        {
            var svc = new AuditService(new AuditOptions { LogFailedLogins = false });
            var result = svc.LogFailedLogin("user-1", "127.0.0.1");
            Assert.Null(result.UserId);
        }

        // LogTokenIssued

        [Fact]
        public void LogTokenIssued_ShouldReturnEvent_WhenEnabled()
        {
            var result = _auditService.LogTokenIssued("user-1", "access");
            Assert.Equal(AuditEventType.TokenIssued, result.EventType);
            Assert.True(result.Success);
        }

        [Fact]
        public void LogTokenIssued_ShouldReturnEmptyEvent_WhenLogTokenUsageIsDisabled()
        {
            var svc = new AuditService(new AuditOptions { LogTokenUsage = false });
            var result = svc.LogTokenIssued("user-1", "access");
            Assert.Null(result.UserId);
        }

        // LogTokenRefreshed

        [Fact]
        public void LogTokenRefreshed_ShouldReturnEvent_WhenEnabled()
        {
            var result = _auditService.LogTokenRefreshed("user-1");
            Assert.Equal(AuditEventType.TokenRefreshed, result.EventType);
            Assert.True(result.Success);
        }

        [Fact]
        public void LogTokenRefreshed_ShouldReturnEmpty_WhenLogTokenUsageIsFalse()
        {
            var svc = new AuditService(new AuditOptions { LogTokenUsage = false });
            var result = svc.LogTokenRefreshed("user-1");
            Assert.Equal(new AuditEvent().EventType, result.EventType);
        }

        // LogTokenRevoked

        [Fact]
        public void LogTokenRevoked_ShouldReturnEvent_WhenEnabled()
        {
            var result = _auditService.LogTokenRevoked("user-1", "Suspicious activity");
            Assert.Equal(AuditEventType.TokenRevoked, result.EventType);
            Assert.True(result.Success);
        }

        [Fact]
        public void LogTokenRefreshed_ShouldReturnEmptyEvent_WhenLogTokenUsageIsDisabled()
        {
            var svc = new AuditService(new AuditOptions { LogTokenUsage = false });
            var result = svc.LogTokenRefreshed("user-1");
            Assert.Null(result.UserId);
        }

        // LogPasswordChanged

        [Fact]
        public void LogPasswordChanged_ShouldReturnEvent_WhenEnabled()
        {
            var result = _auditService.LogPasswordChanged("user-1");
            Assert.Equal(AuditEventType.PasswordChanged, result.EventType);
            Assert.True(result.Success);
        }

        [Fact]
        public void LogPasswordChanged_ShouldReturnEmptyEvent_WhenLogPasswordChangesIsDisabled()
        {
            var svc = new AuditService(new AuditOptions { LogPasswordChanges = false });
            var result = svc.LogPasswordChanged("user-1");
            Assert.Null(result.UserId);
        }

        // LogPasswordResetRequested

        [Fact]
        public void LogPasswordResetRequested_ShouldReturnEvent_WhenEnabled()
        {
            var result = _auditService.LogPasswordResetRequested("user-1");
            Assert.Equal(AuditEventType.PasswordResetRequested, result.EventType);
            Assert.True(result.Success);
        }

        [Fact]
        public void LogPasswordResetRequested_ShouldReturnEmptyEvent_WhenLogPasswordChangesIsDisabled()
        {
            var svc = new AuditService(new AuditOptions { LogPasswordChanges = false });
            var result = svc.LogPasswordResetRequested("user-1");
            Assert.Null(result.UserId);
        }

        // LogSuspiciousActivity

        [Fact]
        public void LogSuspiciousActivity_ShouldReturnEvent_WhenEnabled()
        {
            var result = _auditService.LogSuspiciousActivity("user-1", "127.0.0.1", "Multiple failed attempts");
            Assert.Equal(AuditEventType.SuspiciousActivity, result.EventType);
            Assert.False(result.Success);
        }

        [Fact]
        public void LogSuspiciousActivity_ShouldReturnEmptyEvent_WhenLogSuspiciousActivityIsDisabled()
        {
            var svc = new AuditService(new AuditOptions { LogSuspiciousActivity = false });
            var result = svc.LogSuspiciousActivity("user-1", "127.0.0.1", "Multiple failed attempts");
            Assert.Null(result.UserId);
        }

        // LogLogout

        [Fact]
        public void LogLogout_ShouldReturnEvent_WhenEnabled()
        {
            var result = _auditService.LogLogout("user-1");
            Assert.Equal(AuditEventType.Logout, result.EventType);
            Assert.True(result.Success);
        }

        [Fact]
        public void LogLogout_ShouldReturnEmptyEvent_WhenAuditingIsDisabled()
        {
            var svc = new AuditService(new AuditOptions { Enabled = false });
            var result = svc.LogLogout("user-1");
            Assert.Null(result.UserId);
        }
    }
}