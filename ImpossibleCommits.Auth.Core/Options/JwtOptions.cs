namespace ImpossibleCommits.Auth.Core.Options
{
    public class JwtOptions
    {
        public string Secret { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public TimeSpan AccessTokenExpiry { get; set; } = TimeSpan.FromMinutes(15);
        public TimeSpan RefreshTokenExpiry { get; set; } = TimeSpan.FromDays(7);
        public bool EnableRotation { get; set; } = true;
        public bool EnableTokenBinding { get; set; } = false;
    }
}