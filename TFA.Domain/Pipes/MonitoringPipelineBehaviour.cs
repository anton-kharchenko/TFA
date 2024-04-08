using MediatR;
using Microsoft.Extensions.Logging;
using TFA.Domain.Interfaces.Monitoring;
using TFA.Domain.Monitoring;

namespace TFA.Domain.Pipes;

internal class MonitoringPipelineBehaviour<TRequest, TResponse>(DomainMetrics metrics, ILogger<MonitoringPipelineBehaviour<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request is not IMonitoringRequest monitoringRequest) return next.Invoke();

        try
        {
            var result = next.Invoke();
            monitoringRequest.MonitorSuccess(metrics);
            return result;
        }
        catch (Exception e)
        {
            monitoringRequest.MonitorFailure(metrics);
            logger.LogError(e, "MetricsPipelineBehaviour error caught");
            throw;
        }
    }
}