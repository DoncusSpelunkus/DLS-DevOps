using Microsoft.EntityFrameworkCore;
using PatientService.DataAccess.Interfaces;
using PatientService.DataAccess.Repositories;
using PatientService.Services.Interface;
using FeatureHubSDK;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://0.0.0.0:8081");
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSingleton<IFeatureHubContext, FeatureHubContextService>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IPatientService,PatientService.Services.PatientService>();

builder.Services.AddDbContext<PatientDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PatientDb")));
var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

await app.RunAsync();
