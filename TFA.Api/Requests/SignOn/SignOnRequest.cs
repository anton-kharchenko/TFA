namespace TFA.Api.Requests.SignOn;

public class SignOnRequest
{
    public required string Login { get; set; }
    
    public required string Password { get; set; }
}