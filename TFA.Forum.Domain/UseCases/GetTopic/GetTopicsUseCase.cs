using FluentValidation;
using MediatR;
using TFA.Forum.Domain.Extensions.UseCases;
using TFA.Forum.Domain.Interfaces.UseCases.GetForums;
using TFA.Forum.Domain.Interfaces.UseCases.GetTopics;
using TFA.Forum.Domain.Models;
using TFA.Forum.Domain.Queries.GetTopics;

namespace TFA.Forum.Domain.UseCases.GetTopic;

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