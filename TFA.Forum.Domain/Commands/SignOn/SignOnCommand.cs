using System.Security.Principal;
using MediatR;
using TFA.Forum.Domain.Interfaces.Monitoring;
using TFA.Forum.Domain.Keys;
using TFA.Forum.Domain.Monitoring;

namespace TFA.Forum.Domain.Commands.SignOn;

public record SignOnCommand(string Login, string Password) : IRequest<IIdentity>, IRequest<Interfaces.Authentication.IIdentity>, IMonitoringRequest
{
    public void MonitorSuccess(DomainMetrics metrics) => 
        metrics.IncrementCounter(MonitoringKeys.UserSignOnCounterName, 1, DomainMetrics.ResultTags(true));

    public void MonitorFailure(DomainMetrics metrics) => 
        metrics.IncrementCounter(MonitoringKeys.UserSignOnCounterName, 1, DomainMetrics.ResultTags(false));
}