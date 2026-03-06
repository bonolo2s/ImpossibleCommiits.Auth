namespace ImpossibleCommits.Auth.Core.Options
{
    public class AuditOptions
    {
        public bool Enabled { get; set; } = true;
        public bool LogSuccessfulLogins { get; set; } = true;
        public bool LogFailedLogins { get; set; } = true;
        public bool LogTokenUsage { get; set; } = true;
        public bool LogPasswordChanges { get; set; } = true;
        public bool LogSuspiciousActivity { get; set; } = true;
    }
}