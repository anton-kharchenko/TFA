using MediatR;
using TFA.Domain.Interfaces.Monitoring;
using TFA.Domain.Keys;
using TFA.Domain.Models;
using TFA.Domain.Monitoring;

namespace TFA.Domain.Queries.GetForum;

public record GetForumQuery : IRequest<IEnumerable<Forum>>, IMonitoringRequest
{
    public void MonitorSuccess(DomainMetrics metrics) => 
        metrics.IncrementCounter(MonitoringKeys.ForumFetchCounterName, 1, DomainMetrics.ResultTags(true));

    public void MonitorFailure(DomainMetrics metrics) => 
        metrics.IncrementCounter(MonitoringKeys.ForumFetchCounterName, 1, DomainMetrics.ResultTags(false));
}
