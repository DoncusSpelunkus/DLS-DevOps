using System.Diagnostics;
using System.Reflection;
using Serilog;
using Serilog.Enrichers.Span;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Monitoring;

public static class Monitoring
{
    public static readonly string serviceName = Assembly.GetCallingAssembly().GetName().Name ?? "Unknown";
    public static ActivitySource activitySource = new ActivitySource(serviceName);
    public static TracerProvider TracerProvider;
    public static ILogger Log => Serilog.Log.Logger;

    static Monitoring()
    {
        ConfigureSerilog();
        ConfigureOpenTelemetry();
    }

    public static void ConfigureSerilog()
    {
        Serilog.Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Console()
            .WriteTo.Seq("http://seq:5341")
            .Enrich.WithSpan()
            .CreateLogger();
    }

    public static void ConfigureOpenTelemetry()
    {
        TracerProvider = Sdk.CreateTracerProviderBuilder()
            .AddZipkinExporter(options =>
            {
                options.Endpoint = new Uri("http://zipkin:9411/api/v2/spans"); 
            })
            .AddSource(activitySource.Name)
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName))
            .Build();
    }
}