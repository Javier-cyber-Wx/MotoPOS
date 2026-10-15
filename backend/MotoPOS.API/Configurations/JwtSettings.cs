namespace MotoPOS.API.Configurations
{
    public class JwtSettings
    {
        public string SecretKey { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        
        [Obsolete("Use AccessTokenExpirationInMinutes instead")]
        public int ExpirationInMinutes { get; set; }
        
        public int AccessTokenExpirationInMinutes { get; set; } = 30;
        public int RefreshTokenExpirationInDays { get; set; } = 7;
    }
}