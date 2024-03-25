using FluentAssertions;
using TFA.Domain.UseCases.SignOn;
using TFA.Domain.Validations.SignOn;

namespace TFA.Domain.Tests.SignOn;

public class SignOnCommandValidatorShould
{
    private readonly SignOnCommandValidator sut = new();

    [Fact]
    public void ReturnSuccess_WhenCommandValid()
    {
        var validCommand = new SignOnCommand("Hi", "qwerty");
        sut.Validate(validCommand).IsValid.Should().BeTrue();
    }

    public static IEnumerable<object[]> GetInvalidCommand()
    {
        var validCommand = new SignOnCommand("Hi", "qwerty");
        yield return [validCommand with { Login = string.Empty }];
        yield return [validCommand with { Login = "de67f907-c96b-4bd2-888d-b4be34fdff1af1c00f9b-03ff-46fa-8c5e-7b64a9378c123ee388e3-faf9-4007-b2fe-6ed8bfb4cf32" }];
        yield return [validCommand with { Login = "          " }];
        yield return [validCommand with { Password = string.Empty}];
        yield return [validCommand with { Password = "          " }];
    }

    [Theory]
    [MemberData(nameof(GetInvalidCommand))]
    public void ReturnFailure_WhenCommandInvalid(SignOnCommand command)
    {
        sut.Validate(command).IsValid.Should().BeFalse();
    }
}