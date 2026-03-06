namespace ImpossibleCommits.Auth.Core.Models
{
    public class AuditEvent
    {
        public AuditEventType EventType { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string IpAddress { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public bool Success { get; set; }
        public string Details { get; set; } = string.Empty;
    }
}