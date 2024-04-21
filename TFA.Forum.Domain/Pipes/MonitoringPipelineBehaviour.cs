using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;
using TFA.Forum.Domain.Interfaces.Monitoring;
using TFA.Forum.Domain.Monitoring;

namespace TFA.Forum.Domain.Pipes;


internal class MonitoringPipelineBehaviour<TRequest, TResponse>(
    DomainMetrics metrics,
    ILogger<MonitoringPipelineBehaviour<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (request is not IMonitoringRequest monitoringRequest)
            return await next.Invoke();


        using var activity = DomainMetrics.ActivitySource.StartActivity(
            "usecase", ActivityKind.Internal, default(ActivityContext));
        activity?.AddTag("tfa.command", request.GetType().Name);

        try
        {
            var result = await next.Invoke();
            monitoringRequest.MonitorSuccess(metrics);
            activity?.AddTag("error", false);
            return result;
        }
        catch (Exception e)
        {
            monitoringRequest.MonitorFailure(metrics);
            activity?.AddTag("error", true);
            logger.LogError(e, "MetricsPipelineBehaviour error caught");
            throw;
        }
    }
}