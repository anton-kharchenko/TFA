namespace TFA.Api.Requests.SignIn;

public class SignInRequest
{
    public required string Login { get; set; }
    
    public required string Password { get; set; }
}