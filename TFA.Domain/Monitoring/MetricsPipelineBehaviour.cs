using MediatR;
using TFA.Domain.Interfaces.Monitoring;

namespace TFA.Domain.Monitoring;

internal class MetricsPipelineBehaviour<TRequest, TResponse>(DomainMetrics metrics) 
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
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
            Console.WriteLine(e);
            throw;
        }
    }
}