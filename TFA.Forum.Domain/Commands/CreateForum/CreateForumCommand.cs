using MediatR;
using TFA.Forum.Domain.Models;
using TFA.Forum.Domain.Interfaces.Monitoring;
using TFA.Forum.Domain.Keys;
using TFA.Forum.Domain.Monitoring;

namespace TFA.Forum.Domain.Commands.CreateForum;

public record CreateForumCommand(string Title) : IRequest<Models.Forum>, IMonitoringRequest
{
    public void MonitorSuccess(DomainMetrics metrics) => 
        metrics.IncrementCounter(MonitoringKeys.ForumCreateCounterName, 1, DomainMetrics.ResultTags(true));

    public void MonitorFailure(DomainMetrics metrics) => 
        metrics.IncrementCounter(MonitoringKeys.ForumCreateCounterName, 1, DomainMetrics.ResultTags(false));
}