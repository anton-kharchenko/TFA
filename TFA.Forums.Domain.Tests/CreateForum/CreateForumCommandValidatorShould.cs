using FluentAssertions;
using TFA.Forums.Domain.Commands.CreateForum;
using TFA.Forums.Domain.Validations.Commands.CreateForum;

namespace TFA.Forums.Domain.Tests.CreateForum;

public class CreateForumCommandValidatorShould
{
    private readonly CreateForumCommandValidator sut = new();

    [Fact]
    public void ReturnSuccess_WhenCommandValid()
    {
        var validCommand = new CreateForumCommand("Title");
        sut.Validate(validCommand).IsValid.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(GetInvalidCommands))]
    public void ReturnFailure_WhenCommandInvalid(CreateForumCommand command)
    {
        sut.Validate(command).IsValid.Should().BeFalse();
    }

    public static IEnumerable<object[]> GetInvalidCommands()
    {
        yield return [new CreateForumCommand(string.Empty)];
        yield return
        [
            new CreateForumCommand(
                "300CF5B6-BDC9-89DF-B794-0D848488BB3A8CE32220-954B-82FF-8E27-CCB9C98D5CFE39CABBB3-0213-8C25-9C56-3F03C74F4645")
        ];
    }
}