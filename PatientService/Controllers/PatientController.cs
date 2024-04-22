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
    
}