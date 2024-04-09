using MediatR;
using TFA.Domain.Interfaces.Monitoring;
using TFA.Domain.Keys;
using TFA.Domain.Monitoring;

namespace TFA.Domain.Commands.SignOut;

public record SignOutCommand : IRequest, IMonitoringRequest
{
    public void MonitorSuccess(DomainMetrics metrics) => 
        metrics.IncrementCounter(MonitoringKeys.UserSignOutCounterName, 1, DomainMetrics.ResultTags(true));

    public void MonitorFailure(DomainMetrics metrics) => 
        metrics.IncrementCounter(MonitoringKeys.UserSignOutCounterName, 1, DomainMetrics.ResultTags(false));
}