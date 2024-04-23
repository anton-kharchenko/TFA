namespace TFA.Forums.Domain.Authentication;

public class Session
{
    public Guid UserId { get; set; }

    public DateTimeOffset ExpireAt { get; set; }
}