using FluentValidation;
using TFA.Domain.Commands.GetTopics;
using TFA.Domain.Extensions.UseCases;
using TFA.Domain.Interfaces.UseCases.GetForums;
using TFA.Domain.Interfaces.UseCases.GetTopics;
using TFA.Domain.Models;

namespace TFA.Domain.UseCases.GetTopic;

internal class GetTopicsUseCase(IValidator<GetTopicsQuery> validator, IGetTopicsStorage storage, IGetForumsStorage getTopicsStorage) : IGetTopicsUseCase
{
    public async Task<(IEnumerable<Topic> resources, int totalCount)> ExecuteAsync(GetTopicsQuery query, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(query, cancellationToken);
        await getTopicsStorage.ThrowIfForumNotFoundAsync(query.ForumId, cancellationToken);
        return await storage.GetTopicsAsync(query.ForumId, query.Skip, query.Take, cancellationToken);
    }
}