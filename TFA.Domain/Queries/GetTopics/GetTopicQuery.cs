using MediatR;
using TFA.Domain.Interfaces.Monitoring;
using TFA.Domain.Keys;
using TFA.Domain.Models;
using TFA.Domain.Monitoring;

namespace TFA.Domain.Queries.GetTopics;

public record GetTopicQuery(Guid ForumId, int Skip, int Take):
     IRequest<(IEnumerable<Topic> resources, int totalCount)>, IMonitoringRequest
{
    public void MonitorSuccess(DomainMetrics metrics) => 
        metrics.IncrementCounter(MonitoringKeys.TopicsFetchCounterName, 1, DomainMetrics.ResultTags(true));

    public void MonitorFailure(DomainMetrics metrics) => 
        metrics.IncrementCounter(MonitoringKeys.TopicsFetchCounterName, 1, DomainMetrics.ResultTags(false));
}