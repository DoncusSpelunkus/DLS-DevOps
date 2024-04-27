using AutoMapper;
using DefaultNamespace;
using MeasurementService;
using Microsoft.EntityFrameworkCore;
using Monitoring;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://0.0.0.0:8082");
var serviceName = "MeasurementService";
var serviceVersion = "1.0.0";

builder.Services.AddOpenTelemetry().Setup(serviceName, serviceVersion);
builder.Services.AddSingleton(TracerProvider.Default.GetTracer(serviceName));

builder.Services.AddHttpClient("HttpClient", client =>
{
    client.BaseAddress = new Uri("http://dls-devops-PatientService-1:8081/");
});
builder.Services.AddDbContext<MeasurementDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MeasurementDb")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
var config = new MapperConfiguration(conf =>
{
    conf.CreateMap<MeasurementDto, Measurement>();
});
builder.Services.AddSingleton(config.CreateMapper());
builder.Services.AddSingleton<IFeatureHubContext, FeatureHubContextService>();
builder.Services.AddScoped<IMeasurementRepo, MeasurementRepo>();
builder.Services.AddScoped<IMeasurementService, MeasurementService.MeasurementService>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

await app.RunAsync();