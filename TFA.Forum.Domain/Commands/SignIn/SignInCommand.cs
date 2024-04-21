using MediatR;
using TFA.Forum.Domain.Interfaces.Monitoring;
using TFA.Forum.Domain.Keys;
using TFA.Forum.Domain.Monitoring;

namespace TFA.Forum.Domain.Commands.SignIn;

public record SignInCommand(string Login, string Password) : IRequest<(Interfaces.Authentication.IIdentity identity, string token)>, IMonitoringRequest
{
    public void MonitorSuccess(DomainMetrics metrics) => 
        metrics.IncrementCounter(MonitoringKeys.UserSignInCounterName, 1, DomainMetrics.ResultTags(true));

    public void MonitorFailure(DomainMetrics metrics) => 
        metrics.IncrementCounter(MonitoringKeys.UserSignInCounterName, 1, DomainMetrics.ResultTags(false));
}