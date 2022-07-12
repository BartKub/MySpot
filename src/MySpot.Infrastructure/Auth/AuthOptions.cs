namespace MySpot.Infrastructure.Auth;

public sealed class AuthOptions
{
    public string SigningKey { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public TimeSpan? Expiry { get; set; }
}