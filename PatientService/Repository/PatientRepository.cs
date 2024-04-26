using DefaultNamespace;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Trace;

namespace PatientRepositories;

public class PatientRepository : IPatientRepository
{
    private readonly PatientDbContext _patientDbContext;
    private readonly Tracer _tracer;
    public PatientRepository(PatientDbContext patientDbContext, Tracer tracer)
    {
        _patientDbContext = patientDbContext;
        _tracer = tracer;
    }

    public async Task<Patient?> Create(Patient patient)
    {
        try
        {   using var activity = _tracer.StartActiveSpan("CreatePatient in the repo");
            await _patientDbContext.PatientsTable!.AddAsync(patient);
            await _patientDbContext.SaveChangesAsync();
            return patient;
        }
        catch (Exception e)
        {
            Monitoring.Monitoring.Log.Error("Unable to Create patient in repo.");
            throw new Exception("Something went wrong when creating a new Patients" + e.Message);
        }
    }

    public async Task<Patient?> GetPatientById(string id)
    {
        try
        {
            using var activity = _tracer.StartActiveSpan("GetPatientById in the repo");
            var patient = await _patientDbContext.PatientsTable!.FindAsync(id) ?? throw new KeyNotFoundException("No such patient found");
            return patient;

        }
        catch (Exception e)
        {
            Monitoring.Monitoring.Log.Error("Unable to GetPatientById in repo.");
            throw new Exception("Something went wrong when deleting Patients with id: " +id + e.Message);
            
        }
    }

    public async Task Delete(string id)
    {
        try
        {
            using var activity = _tracer.StartActiveSpan("Delete in the repo");
            var patient = await _patientDbContext.PatientsTable!.FindAsync(id) ?? throw new KeyNotFoundException("No such patient found");
            _patientDbContext.PatientsTable.Remove(patient);
            await _patientDbContext.SaveChangesAsync();
            
        }
        catch (Exception e)
        {
            Monitoring.Monitoring.Log.Error("Unable to Delete patient in repo.");
            throw new Exception("Something went wrong when deleting Patients with id: " +id + e.Message);
            
        }
    }

    public async Task<Patient?> Update(Patient patient)
    {
        try
        {
            using var activity = _tracer.StartActiveSpan("Update in the repo");
            var existingPatient = await _patientDbContext.PatientsTable.FindAsync(patient.Ssn);
            if (existingPatient == null)
            {
                throw new ArgumentException("Post not found", nameof(patient));
            }

            existingPatient.Mail = patient.Mail;
            existingPatient.Name = patient.Mail;
            await _patientDbContext.SaveChangesAsync();
            return existingPatient;
        }
        catch (Exception e)
        {
            Monitoring.Monitoring.Log.Error("Unable to update patient in repo.");
            throw new Exception("Something went wrong when updating Patients: "  + e.Message);
        }
    }

    
    public async Task<List<Patient>> GetAllPatients()
    {
        try
        {
            using var activity = _tracer.StartActiveSpan("GetAllPatients in the repo");
            return await _patientDbContext.PatientsTable!.ToListAsync();
        }
        catch (Exception e)
        {
            Monitoring.Monitoring.Log.Error("Unable to GetAllPatients in repo.");
            throw new Exception("Something went wrong when getting a list of all Patients: "  + e.Message);
        }
    }
    
    public void RebuildDb()
    {
        try
        {
            using var activity = _tracer.StartActiveSpan("RebuildDb in the repo");
            _patientDbContext.Database.EnsureDeleted();
            _patientDbContext.Database.EnsureCreated();
        }
        catch (Exception e)
        {
            Monitoring.Monitoring.Log.Error("Unable to RebuildDb in repo.");
            throw new Exception(e.Message);
        }

    }
}