using MediatR;
using TFA.Forum.Domain.Interfaces.Monitoring;
using TFA.Forum.Domain.Keys;
using TFA.Forum.Domain.Monitoring;

namespace TFA.Forum.Domain.Commands.SignOut;

public record SignOutCommand : IRequest, IMonitoringRequest
{
    public void MonitorSuccess(DomainMetrics metrics) => 
        metrics.IncrementCounter(MonitoringKeys.UserSignOutCounterName, 1, DomainMetrics.ResultTags(true));

    public void MonitorFailure(DomainMetrics metrics) => 
        metrics.IncrementCounter(MonitoringKeys.UserSignOutCounterName, 1, DomainMetrics.ResultTags(false));
}