using ImpossibleCommits.Auth.Core.Options;

public class ImpossibleAuthOptions
{
    public JwtOptions Jwt { get; set; } = new();
    public PasswordOptions Password { get; set; } = new();
    public RateLimitOptions RateLimit { get; set; } = new();
    public AuditOptions Audit { get; set; } = new();
}