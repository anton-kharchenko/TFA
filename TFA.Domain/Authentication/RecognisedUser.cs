namespace TFA.Domain.Authentication;

public class RecognisedUser
{
    public required Guid UserId { get; set; }
    
    public required string Salt { get; set; }
    
    public required string PasswordHash { get; set; }
}