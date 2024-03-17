namespace TFA.Domain.Authentication;

public class RecognisedUser
{
    public  Guid UserId { get; set; }

    public string Salt { get; set; } = default!;

    public string PasswordHash { get; set; } = default!;
}