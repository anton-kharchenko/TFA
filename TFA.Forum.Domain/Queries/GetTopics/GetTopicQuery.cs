using MediatR;
using TFA.Forum.Domain.Interfaces.Monitoring;
using TFA.Forum.Domain.Keys;
using TFA.Forum.Domain.Models;
using TFA.Forum.Domain.Monitoring;

namespace TFA.Forum.Domain.Queries.GetTopics;

public record GetTopicQuery(Guid ForumId, int Skip, int Take):
     IRequest<(IEnumerable<Topic> resources, int totalCount)>, IMonitoringRequest
{
    public void MonitorSuccess(DomainMetrics metrics) => 
        metrics.IncrementCounter(MonitoringKeys.TopicsFetchCounterName, 1, DomainMetrics.ResultTags(true));

    public void MonitorFailure(DomainMetrics metrics) => 
        metrics.IncrementCounter(MonitoringKeys.TopicsFetchCounterName, 1, DomainMetrics.ResultTags(false));
}