using System.Diagnostics;
using System.Reflection;
using Serilog;

namespace Monitoring;

public static class Monitoring
{
    public static readonly string serviceName = Assembly.GetCallingAssembly().GetName().Name ?? "Unknown";
    public static ActivitySource activitySource = new ActivitySource(serviceName);
    public static ILogger Log => Serilog.Log.Logger;

    static Monitoring()
    {
        Serilog.Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Console()
            .WriteTo.Seq("http://seq:5341")
            .CreateLogger();
    }
    public static void Main(string[] args)
    {
        Console.WriteLine("Monitoring service is running...");
    }
    
}