namespace TFA.Domain.Share;

public class RecognisedUser
{
    public  Guid UserId { get; set; }

    public byte[] Salt { get; set; } = default!;

    public byte[] PasswordHash { get; set; } = default!;
}