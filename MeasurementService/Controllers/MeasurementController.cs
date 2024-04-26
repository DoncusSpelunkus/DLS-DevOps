using DefaultNamespace;
using Microsoft.AspNetCore.Mvc;
using Globalization;

namespace MeasurementService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MeasurementController : ControllerBase
    {
        private readonly IMeasurementService _measurementService;

        private readonly IFeatureHubContext _featureHubContext;


        public MeasurementController(
            IMeasurementService measurementService,
            IFeatureHubContext featureHubContext
        )
        {
            _measurementService = measurementService;
            _featureHubContext = featureHubContext;

        }

        [HttpGet("GetAll/{ssn}")]
        public async Task<IActionResult> GetAllMeasurements(string ssn)
        {
            var enabled = await getDeploymentToggle();
            if(!enabled)
            {
                return BadRequest("Feature is disabled");
            }

            var measurements = await _measurementService.GetAllMeasurement(ssn);
            return Ok(measurements);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetMeasurementById(int id)
        {
            var enabled = await getDeploymentToggle();
            if(!enabled)
            {
                return BadRequest("Feature is disabled");
            }

            var measurement = await _measurementService.GetMeasurementById(id);
            if (measurement == null)
                return NotFound($"No measurement found with ID {id}.");

            return Ok(measurement);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateMeasurement(MeasurementDto measurementDto, [FromHeader(Name = "country")] string country)
        {

            var countryName = CountryConverter.GetCountryName(country);

            var config = await _featureHubContext.GetFeatureHubContextAsync();

            var context = await config.NewContext().Attr("country", countryName.ToLower()).Build();
            
            var enbabled = context["CreateMeasurement"].IsEnabled;
            if(!enbabled)
            {
                return BadRequest("Feature is disabled");
            }

            var createdMeasurement = await _measurementService.CreateMeasurement(measurementDto);
            if (createdMeasurement == null)
                return BadRequest("Unable to create measurement.");

            return Ok(createdMeasurement);
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateMeasurement(MeasurementDto measurementDto, int id)
        {
            
            var enabled = await getDeploymentToggle();
            if(!enabled)
            {
                return BadRequest("Feature is disabled");
            }

            var updatedMeasurement = await _measurementService.UpdateMeasurement(
                measurementDto,
                id
            );
            if (updatedMeasurement == null)
                return BadRequest("Unable to update measurement.");

            return Ok(updatedMeasurement);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteMeasurement(int id)
        {
            var enabled = await getDeploymentToggle();
            if(!enabled)
            {
                return BadRequest("Feature is disabled");
            }
            await _measurementService.DeleteMeasurement(id);
            return NoContent();
        }

        [HttpPost("RebuildDb")]
        public IActionResult RebuildDb()
        {
            var enabled = getDeploymentToggle().Result;
            if(!enabled)
            {
                return BadRequest("Feature is disabled");
            }
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
