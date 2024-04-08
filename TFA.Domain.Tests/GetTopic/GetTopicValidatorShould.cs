using FluentAssertions;
using TFA.Domain.Commands.GetTopics;
using TFA.Domain.Validations.Queries.GetTopics;

namespace TFA.Domain.Tests.GetTopic;

public class GetTopicValidatorShould
{
    private readonly GetTopicQueryValidator sut = new();

    [Fact]
    public void ReturnSuccess_WhenQueryIsValid()
    {
        var getTopicsQuery = new GetTopicsQuery(Guid.Parse("123cce62-3af1-456b-bb07-8d22a4d9b343"),
            10,
            5);
        sut.Validate(getTopicsQuery).IsValid.Should().BeTrue();
    }

    public static IEnumerable<object[]> GetInvalidQuery()
    {
        var query = new GetTopicsQuery(Guid.Parse("123cce62-3af1-456b-bb07-8d22a4d9b343"),
            10,
            5);
        yield return [query with { ForumId = Guid.Empty }];
        yield return [query with { Skip = -40 }];
        yield return [query with { Take = -40 }];
    }

    [Theory]
    [MemberData(nameof(GetInvalidQuery))]
    public void ReturnFailure_WhenQueryIsInvalid(GetTopicsQuery query)
    {
        sut.Validate(query).IsValid.Should().BeFalse();
    }
}