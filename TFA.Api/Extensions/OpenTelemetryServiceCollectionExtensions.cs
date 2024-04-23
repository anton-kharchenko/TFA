// Ignore Spelling: Api

using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace TFA.Api.Extensions;

internal static class OpenTelemetryServiceCollectionExtensions
{
    public static void AddApiMetrics(this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddOpenTelemetry()
            .WithMetrics(builder => builder
                .AddInstrumentation(serviceProvider => serviceProvider)
                .AddPrometheusExporter()
                .AddMeter("TFA.Forums.Domain")
            )
            .WithTracing(builder => builder.ConfigureResource(r => r.AddService("TFA"))
                .AddAspNetCoreInstrumentation(opt =>
                {
                    opt.Filter += context =>
                        !context.Request.Path.Value!.Contains("metrics", StringComparison.InvariantCulture) &&
                        !context.Request.Path.Value!.Contains("swagger", StringComparison.InvariantCulture);
                    opt.EnrichWithHttpResponse = (activity, response) =>
                        activity.AddTag("error", response.StatusCode = 400);
                })
                .AddEntityFrameworkCoreInstrumentation(cnf => cnf.SetDbStatementForText = true)
                .AddSource("TFA.Forums.Domain")
                .AddConsoleExporter()
                .AddJaegerExporter(options => options.Endpoint = new Uri(configuration.GetConnectionString("Tracing")!))
            );
    }
}