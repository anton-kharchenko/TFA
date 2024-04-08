using FluentAssertions;
using TFA.Domain.Commands.SignIn;
using TFA.Domain.UseCases.SignIn;
using TFA.Domain.Validations.Authentications.SignIn;

namespace TFA.Domain.Tests.SignIn;

public class SignInCommandValidatorShould
{
    private readonly SignInCommandValidator sut = new();

    [Fact]
    public void ReturnSuccess_WhenCommandValid()
    {
        var command = new SignInCommand("Test", "qwerty");
        sut.Validate(command).IsValid.Should().BeTrue();
    }

    public static IEnumerable<object[]> GetInvalidCommand()
    {
        var command = new SignInCommand("Test", "qwerty");
        yield return [command with { Login = "      " }];
        yield return [command with { Login = string.Empty }];
        yield return [command with { Login = "123456789012345678901234567890" }];
        yield return [command with { Password = string.Empty }];
        yield return [command with { Password = "         " }];
    }

    [Theory]
    [MemberData(nameof(GetInvalidCommand))]
    public void ReturnFailure_WhenCommandInvalid(SignInCommand command)
    {
        sut.Validate(command).IsValid.Should().BeFalse();
    }
}