using MediatR;
using TFA.Domain.Models;

namespace TFA.Domain.Queries.GetTopics;

public record GetTopicQuery(Guid ForumId, int Skip, int Take): IRequest<(IEnumerable<Topic> resources, int totalCount)>;