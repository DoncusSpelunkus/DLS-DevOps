using System.Threading.Tasks;
using DefaultNamespace;
using Globalization;
using Microsoft.AspNetCore.Mvc;
using OpenTelemetry.Trace;

namespace MeasurementService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MeasurementController : ControllerBase
    {
        private readonly IMeasurementService _measurementService;
        private readonly Tracer _tracer;
        private readonly IFeatureHubContext _featureHubContext;

        public MeasurementController(
            IMeasurementService measurementService,
            IFeatureHubContext featureHubContext, Tracer tracer)
        {
            _measurementService = measurementService;
            _featureHubContext = featureHubContext;
            _tracer = tracer;
        }

        [HttpGet("GetAll/{ssn}")]
        public async Task<IActionResult> GetAllMeasurements(string ssn)
        { 
            
            var enabled = await getDeploymentToggle();
            if (!enabled)
            {
                return BadRequest("Feature is disabled");
            }

            using var activity = _tracer.StartActiveSpan("GetAllMeasurementsInMeasurementController");

            var measurements = await _measurementService.GetAllMeasurement(ssn);
            if (measurements == null)
            {
                Monitoring.Monitoring.Log.Error(
                    "Unable to retrieve measurements in MeasurementController."
                );
                return BadRequest("Unable to retrieve measurements.");
            }

            return Ok(measurements);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetMeasurementById(int id)
        {
            var enabled = await getDeploymentToggle();
            if (!enabled)
            {
                return BadRequest("Feature is disabled");
            }

            using var activity = _tracer.StartActiveSpan(
                "GetMeasurementByIdInMeasurementController"
            );

            var measurement = await _measurementService.GetMeasurementById(id);
            if (measurement == null)
            {
                Monitoring.Monitoring.Log.Error(
                    $"No measurement found with ID {id} in MeasurementController."
                );
                return NotFound($"No measurement found with ID {id}.");
            }
            return Ok(measurement);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateMeasurement(
            MeasurementDto measurementDto,
            [FromHeader(Name = "country")] string country
        )
        {
            try
            {
                var countryName = CountryConverter.GetCountryName(country);

                var config = await _featureHubContext.GetFeatureHubContextAsync();

                var context = await config
                    .NewContext()
                    .Attr("country", countryName.ToLower())
                    .Build();

                Monitoring.Monitoring.Log.Debug(
                    "Country name: " + countryName
                );

                var enbabled = context["CreateMeasurement"].IsEnabled;
                if (!enbabled)
                {
                    return BadRequest("Feature is disabled");
                }
                
                var createdMeasurement = await _measurementService.CreateMeasurement(
                    measurementDto
                );
                if (createdMeasurement == null)
                {
                    Monitoring.Monitoring.Log.Error(
                        "Unable to create measurement in MeasurementController."
                    );
                    return BadRequest("Unable to create measurement.");
                }

                return Ok(createdMeasurement);
            }
            catch (Exception e)
            {
                Monitoring.Monitoring.Log.Error("something went wrong in MeasurementController." + e.Message);
                return BadRequest("Invalid country code");
            }
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateMeasurement(MeasurementDto measurementDto, int id)
        {
            
            var enabled = await getDeploymentToggle();
            if (!enabled)
            {
                return BadRequest("Feature is disabled");
            }
            
            var updatedMeasurement = await _measurementService.UpdateMeasurement(
                measurementDto,
                id
            );

            using var activity = _tracer.StartActiveSpan("UpdateMeasurementInMeasurementController");
            
            if (updatedMeasurement == null)
            {
                Monitoring.Monitoring.Log.Error(
                    "Unable to update measurement in MeasurementController."
                );
                return BadRequest("Unable to update measurement.");
            }
            return Ok(updatedMeasurement);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteMeasurement(int id)
        {
            var enabled = await getDeploymentToggle();
            if (!enabled)
            {
                return BadRequest("Feature is disabled");
            }
            using var activity = _tracer.StartActiveSpan(
                "DeleteMeasurementInMeasurementController"
            );

            await _measurementService.DeleteMeasurement(id);
            return NoContent();
        }

        [HttpPost("RebuildDb")]
        public IActionResult RebuildDb()
        {
            _measurementService.RebuildDb();
            return Ok("Database rebuilt successfully.");
        }

        private async Task<bool> getDeploymentToggle()
        {
            var config = await _featureHubContext.GetFeatureHubContextAsync();

            var context = await config.NewContext().Build();

            return context["Production"].IsEnabled;
        }
    }
}
