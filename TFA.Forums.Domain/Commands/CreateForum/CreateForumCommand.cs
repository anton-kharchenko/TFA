using MediatR;
using TFA.Forums.Domain.Models;
using TFA.Forums.Domain.Interfaces.Monitoring;
using TFA.Forums.Domain.Keys;
using TFA.Forums.Domain.Monitoring;

namespace TFA.Forums.Domain.Commands.CreateForum;

public record CreateForumCommand(string Title) : IRequest<Models.Forum>, IMonitoringRequest
{
    public void MonitorSuccess(DomainMetrics metrics) => 
        metrics.IncrementCounter(MonitoringKeys.ForumCreateCounterName, 1, DomainMetrics.ResultTags(true));

    public void MonitorFailure(DomainMetrics metrics) => 
        metrics.IncrementCounter(MonitoringKeys.ForumCreateCounterName, 1, DomainMetrics.ResultTags(false));
}