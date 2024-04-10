using FluentValidation;
using MediatR;
using TFA.Domain.Extensions.UseCases;
using TFA.Domain.Interfaces.UseCases.GetForums;
using TFA.Domain.Interfaces.UseCases.GetTopics;
using TFA.Domain.Models;
using TFA.Domain.Queries.GetTopics;

namespace TFA.Domain.UseCases.GetTopic;

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