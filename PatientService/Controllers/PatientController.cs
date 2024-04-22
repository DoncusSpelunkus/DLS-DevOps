using Microsoft.AspNetCore.Mvc;
using PatientServices;
[ApiController]
[Route("[controller]")]
public class PatientController : ControllerBase
{
    private readonly IPatientService _patientService;
    private readonly IFeatureHubContext _featureHubContext;
    public PatientController(IPatientService patientService, IFeatureHubContext featureHubContext){
        _patientService = patientService;
        _featureHubContext = featureHubContext;
    }


    [HttpGet]
    public async Task<IActionResult> GetPatients()
    {
        var featureHubContext = await _featureHubContext.GetFeatureHubContextAsync();
        return Ok(featureHubContext["Test"].IsEnabled);
    }

    [HttpPost("Create")]
    public async Task<ActionResult<Patient>> Create(Patient patient)
    {
        var createdPatient = await _patientService.Create(patient);
        if (createdPatient == null)
            return BadRequest("Unable to create patient.");

        return Ok(createdPatient);
    }

    [HttpGet("GetById")]
    public async Task<ActionResult<Patient>> GetPatientById(string id)
    {
        var patient = await _patientService.GetPatientById(id);
        if (patient == null)
            return NotFound($"No patient found with ID {id}.");

        return Ok(patient);
    }

    [HttpDelete("Delete")]
    public async Task<IActionResult> Delete(string id)
    {
        await _patientService.Delete(id);
        return NoContent();
    }

    [HttpPut("UpdatePatient")]
    public async Task<ActionResult<Patient>> Update(Patient patient)
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