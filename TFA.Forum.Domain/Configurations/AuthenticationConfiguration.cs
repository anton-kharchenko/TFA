namespace TFA.Forum.Domain.Configurations;

public class AuthenticationConfiguration
{
    public string Base64Key { get; set; } = default!;

    public byte[] Key => Convert.FromBase64String(Base64Key);
}