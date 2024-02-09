namespace EventsApp
{
    public class JwtSettings
    {
        public string? SecretKey { get; set; }
        public string? DurationInDays { get; set; }
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
    }
}
