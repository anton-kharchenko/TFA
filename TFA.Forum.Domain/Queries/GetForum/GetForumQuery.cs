using MediatR;
using TFA.Forum.Domain.Models;
using TFA.Forum.Domain.Interfaces.Monitoring;
using TFA.Forum.Domain.Keys;
using TFA.Forum.Domain.Monitoring;

namespace TFA.Forum.Domain.Queries.GetForum;

public record GetForumQuery : IRequest<IEnumerable<Models.Forum>>, IMonitoringRequest
{
    public void MonitorSuccess(DomainMetrics metrics) => 
        metrics.IncrementCounter(MonitoringKeys.ForumFetchCounterName, 1, DomainMetrics.ResultTags(true));

    public void MonitorFailure(DomainMetrics metrics) => 
        metrics.IncrementCounter(MonitoringKeys.ForumFetchCounterName, 1, DomainMetrics.ResultTags(false));
}
