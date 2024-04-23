using MediatR;
using TFA.Forums.Domain.Interfaces.Monitoring;
using TFA.Forums.Domain.Keys;
using TFA.Forums.Domain.Monitoring;

namespace TFA.Forums.Domain.Commands.SignIn;

public record SignInCommand(string Login, string Password) : IRequest<(Interfaces.Authentication.IIdentity identity, string token)>, IMonitoringRequest
{
    public void MonitorSuccess(DomainMetrics metrics) => 
        metrics.IncrementCounter(MonitoringKeys.UserSignInCounterName, 1, DomainMetrics.ResultTags(true));

    public void MonitorFailure(DomainMetrics metrics) => 
        metrics.IncrementCounter(MonitoringKeys.UserSignInCounterName, 1, DomainMetrics.ResultTags(false));
}