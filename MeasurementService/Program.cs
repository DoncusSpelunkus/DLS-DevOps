using AutoMapper;
using DefaultNamespace;
using MeasurementService;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://0.0.0.0:8082");

// Add services to the container.
builder.Services.AddDbContext<MeasurementDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MeasurementDb"))); // Use SQLite

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

// AutoMapper configuration
var config = new MapperConfiguration(conf =>
{
    conf.CreateMap<MeasurementDto, Measurement>();
});
builder.Services.AddSingleton(config.CreateMapper());

// Register repositories and services
builder.Services.AddScoped<IMeasurementRepo, MeasurementRepo>();
builder.Services.AddScoped<IMeasurementService, MeasurementService.MeasurementService>();

// Configure Swagger/OpenAPI
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API Title", Version = "v1" });
});

var app = builder.Build();

app.UseHttpsRedirection();

// Enable middleware to serve generated Swagger as a JSON endpoint.
app.UseSwagger();

// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
// specifying the Swagger JSON endpoint.
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
    // Optionally, configure other settings like the UI endpoint.
    // c.RoutePrefix = string.Empty; // To serve the Swagger UI at the app's root (http://localhost:<port>/)
    // c.DocExpansion(DocExpansion.None); // Set the initial expand status of the operation list.
});

app.MapControllers();

await app.RunAsync();