using DefaultNamespace;
using Microsoft.AspNetCore.Mvc;
using PatientServices;
using Monitoring;
using OpenTelemetry.Trace;

[ApiController]
[Route("[controller]")]
public class PatientController : ControllerBase
{
    private readonly IPatientService _patientService;
    private readonly IFeatureHubContext _featureHubContext;
    private readonly Tracer _tracer;
    public PatientController(IPatientService patientService, IFeatureHubContext featureHubContext, Tracer tracer){
        _patientService = patientService;
        _featureHubContext = featureHubContext;
        _tracer = tracer;
    }


    [HttpGet]
    public async Task<IActionResult> GetPatients()
    {   using var activity = _tracer.StartActiveSpan("CreatePatient");
        
        var featureHubContext = await _featureHubContext.GetFeatureHubContextAsync();
        return Ok(featureHubContext["Test"].IsEnabled);
    }

    [HttpPost("Create")]
    public async Task<ActionResult<Patient>> Create(Patient patient)
    {
        
        using var activity = _tracer.StartActiveSpan("CreatePatient");
        var createdPatient = await _patientService.Create(patient);
        if (createdPatient == null)
        {
            Monitoring.Monitoring.Log.Error("Couldn't create the patient");
            return BadRequest("Unable to create patient.");
            
        }
            

        return Ok(createdPatient);
    }

    [HttpGet("GetById")]
    public async Task<ActionResult<Patient>> GetPatientById(string id)
    {   
        using var activity = _tracer.StartActiveSpan("GetPatientById");
        var patient = await _patientService.GetPatientById(id);
        if (patient == null)
        { 
            Monitoring.Monitoring.Log.Error("Couldn't GetPatientById");
            return NotFound($"No patient found with ID {id}.");
        }
            

        return Ok(patient);
    }

    [HttpDelete("Delete")]
    public async Task<IActionResult> Delete(string id)
    {   
        using var activity = _tracer.StartActiveSpan("Delete");
        await _patientService.Delete(id);
        return NoContent();
    }

    [HttpPut("UpdatePatient")]
    public async Task<ActionResult<Patient>> Update(Patient patient, string id)
    {
        
        var updatedPatient = await _patientService.Update(patient);
        if (updatedPatient == null)
            return BadRequest("Unable to update patient.");

        return Ok(updatedPatient);
    }

    [HttpGet("GetAllPatients")]
    public async Task<ActionResult<List<Patient>>> GetAllPatients()
    {
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