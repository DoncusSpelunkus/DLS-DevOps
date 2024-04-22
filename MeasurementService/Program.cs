using MeasurementService.Repository;
using MeasurementService.Repository.DB;
using MeasurementService.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://0.0.0.0:8082");
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
//builder.Services.AddSingleton<IFeatureHubContext, FeatureHubContextService>();
builder.Services.AddScoped<IMeasurementRepo, MeasurementRepo>();
builder.Services.AddScoped<IMeasurementService, MeasurementService.Services.MeasurementService>();

builder.Services.AddDbContext<MeasurementDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MeasurementDb")));
var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

await app.RunAsync();