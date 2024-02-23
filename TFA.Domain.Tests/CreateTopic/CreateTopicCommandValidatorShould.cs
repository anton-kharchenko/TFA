using FluentAssertions;
using TFA.Domain.Commands.CreateTopic;
using TFA.Domain.Validations.CreateTopic;

namespace TFA.Domain.Tests.CreateTopic;

public class CreateTopicCommandValidatorShould
{
    private readonly CreateTopicCommandValidator sut = new();
    
    [Fact]
    public void ReturnSuccess_WhenCommandValid()
    {
       var actual = sut.Validate(new CreateTopicCommand(Guid.Parse("776e6163-7d40-4220-a1d2-5dac70dcfc1e"), "Hello"));
       actual.IsValid.Should().BeTrue();
    }

    public static IEnumerable<object[]> GetInvalidCommand()
    {
        var validCommand = new CreateTopicCommand(Guid.Parse("71a946b7-7bd8-493c-9af9-eefb09654f39"), "Hello");
        
        yield return [validCommand with { ForumId = Guid.Empty}, nameof(CreateTopicCommand.ForumId), "Empty"];
        yield return [validCommand with { Title = string.Empty}, nameof(CreateTopicCommand.ForumId), "Empty"];
        yield return [validCommand with { Title = "       "}, nameof(CreateTopicCommand.ForumId), "Empty"];
        yield return [validCommand with { Title = "NuncconsecteturNuncconsecteturNuncconsecteturNuncconsecteturNunteturonsecteturNuncconsecteturNuncconsectetur"}, 
                      nameof(CreateTopicCommand.ForumId), "TooLong"];
    }

    [Theory]
    [MemberData(nameof(GetInvalidCommand))]
    public void ReturnFailure_WneCommandIsInvalid(CreateTopicCommand command, string expectedInvalidPropertName, string expectedStatusCode)
    {
        var actual = sut.Validate(command);
        actual.IsValid.Should().BeFalse();
        actual.Errors.Should()
            .Contain(f => f.PropertyName == expectedInvalidPropertName && f.ErrorCode == expectedStatusCode);
    } 
}