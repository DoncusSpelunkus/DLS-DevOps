using DefaultNamespace;
using Microsoft.AspNetCore.Mvc;
using OpenTelemetry.Trace;
using System.Threading.Tasks;

namespace MeasurementService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MeasurementController : ControllerBase
    {
        private readonly IMeasurementService _measurementService;
        private readonly Tracer _tracer;

        public MeasurementController(IMeasurementService measurementService, Tracer tracer)
        {
            _measurementService = measurementService;
            _tracer = tracer;
        }

        [HttpGet("GetAll/{ssn}")]
        public async Task<IActionResult> GetAllMeasurements(string ssn)
        {
            using var activity = _tracer.StartActiveSpan("GetAllMeasurementsInMeasurementController");
            var measurements = await _measurementService.GetAllMeasurement(ssn);
            if (measurements == null)
            {
                Monitoring.Monitoring.Log.Error("Unable to retrieve measurements in MeasurementController.");
                return BadRequest("Unable to retrieve measurements.");
            }

            return Ok(measurements);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetMeasurementById(int id)
        {
            using var activity = _tracer.StartActiveSpan("GetMeasurementByIdInMeasurementController");
            var measurement = await _measurementService.GetMeasurementById(id);
            if (measurement == null)
            {
                Monitoring.Monitoring.Log.Error($"No measurement found with ID {id} in MeasurementController.");
                return NotFound($"No measurement found with ID {id}.");
            }

            return Ok(measurement);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateMeasurement(MeasurementDto measurementDto)
        {
            using var activity = _tracer.StartActiveSpan("CreateMeasurementInMeasurementController");
            var createdMeasurement = await _measurementService.CreateMeasurement(measurementDto);
            if (createdMeasurement == null)
            {
                Monitoring.Monitoring.Log.Error("Unable to create measurement in MeasurementController.");
                return BadRequest("Unable to create measurement.");
            }

            return Ok(createdMeasurement);
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateMeasurement(MeasurementDto measurementDto, int id)
        {
            using var activity = _tracer.StartActiveSpan("UpdateMeasurementInMeasurementController");
            var updatedMeasurement = await _measurementService.UpdateMeasurement(measurementDto, id);
            if (updatedMeasurement == null)
            {
                Monitoring.Monitoring.Log.Error("Unable to update measurement in MeasurementController.");
                return BadRequest("Unable to update measurement.");
            }

            return Ok(updatedMeasurement);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteMeasurement(int id)
        {
            using var activity = _tracer.StartActiveSpan("DeleteMeasurementInMeasurementController");
            await _measurementService.DeleteMeasurement(id);
            return NoContent();
        }

        [HttpPost("RebuildDb")]
        public IActionResult RebuildDb()
        {
            using var activity = _tracer.StartActiveSpan("RebuildDbInMeasurementController");
            _measurementService.RebuildDb();
            return Ok("Database rebuilt successfully.");
        }
    }
}
