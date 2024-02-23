using FluentValidation;
using TFA.Domain.Commands.GetTopics;
using TFA.Domain.Interfaces.UseCases.GetTopics;
using TFA.Domain.Models;

namespace TFA.Domain.UseCases.GetTopic;

internal class GetTopicsUseCase(IValidator<GetTopicsQuery> validator, IGetTopicsStorage storage) : IGetTopicsUseCase
{
    public async Task<(IEnumerable<Topic> resources, int totalCount)> ExecuteAsync(GetTopicsQuery query, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(query, cancellationToken);
        return await storage.GetTopicsAsync(query.ForumId, query.Skip, query.Take, cancellationToken);
    }
}