namespace ImpossibleCommits.Auth.Core.Options
{
    public class PasswordOptions
    {
        public int MinLength { get; set; } = 8;
        public bool RequireUppercase { get; set; } = true;
        public bool RequireLowercase { get; set; } = true;
        public bool RequireDigit { get; set; } = true;
        public bool RequireSpecialChar { get; set; } = true;
        public int? ExpiryDays { get; set; } = null;
        public int PasswordHistoryLimit { get; set; } = 5;
    }
}