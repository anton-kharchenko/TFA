namespace TFA.Domain.Configurations;

public class AuthenticationConfiguration
{
    public required string Base64Key { get; set; }

    public IEnumerable<byte> Key => Convert.FromBase64String(Base64Key);
}