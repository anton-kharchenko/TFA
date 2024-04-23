using FluentValidation;
using MediatR;
using TFA.Forums.Domain.Extensions.UseCases;
using TFA.Forums.Domain.Interfaces.UseCases.GetForums;
using TFA.Forums.Domain.Interfaces.UseCases.GetTopics;
using TFA.Forums.Domain.Models;
using TFA.Forums.Domain.Queries.GetTopics;

namespace TFA.Forums.Domain.UseCases.GetTopic;

internal class GetTopicsUseCase(
    IGetTopicsStorage storage,
    IGetForumsStorage getTopicsStorage) : 
    IRequestHandler<GetTopicQuery, (IEnumerable<Topic> resources, int totalCount)>
{
    
    public async Task<(IEnumerable<Topic> resources, int totalCount)> Handle(GetTopicQuery query, CancellationToken cancellationToken)
    {
        await getTopicsStorage.ThrowIfForumNotFoundAsync(query.ForumId, cancellationToken);
        return await storage.GetTopicsAsync(query.ForumId, query.Skip, query.Take, cancellationToken);
    }
}