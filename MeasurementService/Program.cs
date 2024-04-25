using AutoMapper;
using DefaultNamespace;
using MeasurementService;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://0.0.0.0:8082");
builder.Services.AddDbContext<MeasurementDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MeasurementDb")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
var config = new MapperConfiguration(conf =>
{
    conf.CreateMap<MeasurementDto, Measurement>();
});
builder.Services.AddSingleton(config.CreateMapper());
//builder.Services.AddSingleton<IFeatureHubContext, FeatureHubContextService>();
builder.Services.AddScoped<IMeasurementRepo, MeasurementRepo>();
builder.Services.AddScoped<IMeasurementService, MeasurementService.MeasurementService>();

builder.Services.AddDbContext<MeasurementDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MeasurementDb")));
var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

await app.RunAsync();