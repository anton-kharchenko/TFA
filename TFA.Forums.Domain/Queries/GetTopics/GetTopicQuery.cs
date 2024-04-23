using MediatR;
using TFA.Forums.Domain.Interfaces.Monitoring;
using TFA.Forums.Domain.Keys;
using TFA.Forums.Domain.Models;
using TFA.Forums.Domain.Monitoring;

namespace TFA.Forums.Domain.Queries.GetTopics;

public record GetTopicQuery(Guid ForumId, int Skip, int Take):
     IRequest<(IEnumerable<Topic> resources, int totalCount)>, IMonitoringRequest
{
    public void MonitorSuccess(DomainMetrics metrics) => 
        metrics.IncrementCounter(MonitoringKeys.TopicsFetchCounterName, 1, DomainMetrics.ResultTags(true));

    public void MonitorFailure(DomainMetrics metrics) => 
        metrics.IncrementCounter(MonitoringKeys.TopicsFetchCounterName, 1, DomainMetrics.ResultTags(false));
}