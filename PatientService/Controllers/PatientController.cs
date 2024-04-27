using System.Threading.Tasks;
using DefaultNamespace;
using Microsoft.AspNetCore.Mvc;
using OpenTelemetry.Trace;
using PatientServices;

[Route("[controller]")]
public class PatientController : ControllerBase
{
    private readonly IPatientService _patientService;
    private readonly IFeatureHubContext _featureHubContext;
    private readonly Tracer _tracer;

    public PatientController(
        IPatientService patientService,
        IFeatureHubContext featureHubContext,
        Tracer tracer
    )
    {
        _patientService = patientService;
        _featureHubContext = featureHubContext;
        _tracer = tracer;
    }

    [HttpGet]
    public async Task<IActionResult> GetPatients()
    {
        using var activity = _tracer.StartActiveSpan("GetPatientsInPatientController");

        var featureHubContext = await _featureHubContext.GetFeatureHubContextAsync();
        return Ok(featureHubContext["Test"].IsEnabled);
    }

    [HttpPost("Create")]
    public async Task<ActionResult<Patient>> Create([FromBody] Patient patient)
    {
        using var activity = _tracer.StartActiveSpan("CreatePatientInPatientController");
        var createdPatient = await _patientService.Create(patient);
        if (createdPatient == null)
        {
            Monitoring.Monitoring.Log.Error("Couldn't create the patient in PatientController ");
            return BadRequest("Unable to create patient.");
        }

        Monitoring.Monitoring.Log.Debug("Patient created successfully");

        return Ok(createdPatient);
    }

    [HttpGet("GetById/{id}")]
    public async Task<ActionResult<Patient>> GetPatientById(string id)
    {
        using var activity = _tracer.StartActiveSpan("GetPatientByIdInPatientController");
        var patient = await _patientService.GetPatientById(id);
        if (patient == null)
        {
            Monitoring.Monitoring.Log.Error("Couldn't GetPatientById in PatientController");
            return NotFound($"No patient found with ID {id}.");
        }
        return Ok(patient);
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        using var activity = _tracer.StartActiveSpan("DeletePatientInPatientController");
        await _patientService.Delete(id);
        return NoContent();
    }

    [HttpPut("UpdatePatient")]
    public async Task<ActionResult<Patient>> Update(Patient patient, string id)
    {
        using var activity = _tracer.StartActiveSpan("UpdatePatientInPatientController");
        var updatedPatient = await _patientService.Update(patient);
        if (updatedPatient == null)
        {
            Monitoring.Monitoring.Log.Error("Unable to update patient in PatientController.");
            return BadRequest("Unable to update patient.");
        }

        return Ok(updatedPatient);
    }

    [HttpGet("GetAllPatients")]
    public async Task<ActionResult<List<Patient>>> GetAllPatients()
    {
        using var activity = _tracer.StartActiveSpan("GetAllPatientsInPatientController");
        var patients = await _patientService.GetAllPatients();
        return Ok(patients);
    }

    [HttpPost("RebuildDb")]
    public IActionResult RebuildDb()
    {
        
        _patientService.RebuildDb();
        return Ok("Database rebuilt successfully.");
    }
}
