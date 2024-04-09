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
                .AddMeter("TFA.Domain")
            )
            .WithTracing(builder => builder.ConfigureResource(r => r.AddService("TFA"))
                .AddAspNetCoreInstrumentation()
                .AddEntityFrameworkCoreInstrumentation(cnf => cnf.SetDbStatementForText = true)
                .AddSource("TFA.Domain")
                .AddConsoleExporter()
                .AddJaegerExporter(options => options.Endpoint = new Uri(configuration.GetConnectionString("Tracing")!))
            );
    }
}