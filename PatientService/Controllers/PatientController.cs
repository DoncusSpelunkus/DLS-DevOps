using Microsoft.AspNetCore.Mvc;
using PatientService.Services.Interface;

namespace PatientService.Controllers;
[ApiController]
[Route("[controller]")]

public class PatientController : ControllerBase
{
    private readonly IPatientService _patientService;
    public PatientController(IPatientService patientService)
    {
        _patientService = patientService;
    }
    
    
    
}