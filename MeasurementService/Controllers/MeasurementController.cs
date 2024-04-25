
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

        [HttpGet("GetAll/{ssn}")]
        public async Task<IActionResult> GetAllMeasurements(string ssn)
        {
            var measurements = await _measurementService.GetAllMeasurement(ssn);
            return Ok(measurements);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetMeasurementById(int id)
        {
            var measurement = await _measurementService.GetMeasurementById(id);
            if (measurement == null)
                return NotFound($"No measurement found with ID {id}.");
            
            return Ok(measurement);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateMeasurement(MeasurementDto measurementDto)
        {
            var createdMeasurement = await _measurementService.CreateMeasurement(measurementDto);
            if (createdMeasurement == null)
                return BadRequest("Unable to create measurement.");
            
            return Ok(createdMeasurement);
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateMeasurement(MeasurementDto measurementDto, int id)
        {
            var updatedMeasurement = await _measurementService.UpdateMeasurement(measurementDto, id);
            if (updatedMeasurement == null)
                return BadRequest("Unable to update measurement.");
            
            return Ok(updatedMeasurement);
        }

        [HttpDelete("Delete/{id}")]
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
