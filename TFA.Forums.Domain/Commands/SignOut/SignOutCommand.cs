using MediatR;
using TFA.Forums.Domain.Interfaces.Monitoring;
using TFA.Forums.Domain.Keys;
using TFA.Forums.Domain.Monitoring;

namespace TFA.Forums.Domain.Commands.SignOut;

public record SignOutCommand : IRequest, IMonitoringRequest
{
    public void MonitorSuccess(DomainMetrics metrics) => 
        metrics.IncrementCounter(MonitoringKeys.UserSignOutCounterName, 1, DomainMetrics.ResultTags(true));

    public void MonitorFailure(DomainMetrics metrics) => 
        metrics.IncrementCounter(MonitoringKeys.UserSignOutCounterName, 1, DomainMetrics.ResultTags(false));
}