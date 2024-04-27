using Microsoft.EntityFrameworkCore;
using PatientRepositories;
using PatientServices;
using Monitoring;
using OpenTelemetry.Trace;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://0.0.0.0:8081");
var serviceName = "PatientService";
var serviceVersion = "1.0.0";

builder.Services.AddOpenTelemetry().Setup(serviceName, serviceVersion);
builder.Services.AddSingleton(TracerProvider.Default.GetTracer(serviceName));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSingleton<IFeatureHubContext, FeatureHubContextService>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IPatientService, PatientService>();

builder.Services.AddDbContext<PatientDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PatientDb")));

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();

await app.RunAsync();
