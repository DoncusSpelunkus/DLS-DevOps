
using DefaultNamespace;
using Microsoft.AspNetCore.Mvc;

namespace MeasurementService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MeasurementController : ControllerBase
    {
        private readonly IMeasurementService _measurementService;

        public MeasurementController(IMeasurementService measurementService)
        {
            _measurementService = measurementService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMeasurements(string ssn)
        {
            var measurements = await _measurementService.GetAllMeasurement(ssn);
            return Ok(measurements);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMeasurementById(int id, string ssn)
        {
            var measurement = await _measurementService.GetMeasurementById(id, ssn);
            if (measurement == null)
                return NotFound($"No measurement found with ID {id}.");
            
            return Ok(measurement);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateMeasurement(Measurement measurement)
        {
            var createdMeasurement = await _measurementService.CreateMeasurement(measurement);
            if (createdMeasurement == null)
                return BadRequest("Unable to create measurement.");
            
            return Ok(createdMeasurement);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateMeasurement(Measurement measurement)
        {
            var updatedMeasurement = await _measurementService.UpdateMeasurement(measurement);
            if (updatedMeasurement == null)
                return BadRequest("Unable to update measurement.");
            
            return Ok(updatedMeasurement);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMeasurement(int id)
        {
            await _measurementService.DeleteMeasurement(id);
            return NoContent();
        }

        [HttpPost("RebuildDb")]
        public IActionResult RebuildDb()
        {
            _measurementService.RebuildDb();
            return Ok("Database rebuilt successfully.");
        }
    }
}
