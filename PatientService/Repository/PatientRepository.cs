using DefaultNamespace;
using Microsoft.EntityFrameworkCore;

namespace PatientRepositories;

public class PatientRepository : IPatientRepository
{
    private readonly PatientDbContext _patientDbContext;
    public PatientRepository(PatientDbContext patientDbContext)
    {
        _patientDbContext = patientDbContext;
    }

    public async Task<Patient?> Create(Patient patient)
    {
        try
        {
            await _patientDbContext.PatientsTable!.AddAsync(patient);
            await _patientDbContext.SaveChangesAsync();
            return patient;
        }
        catch (Exception e)
        {
            
            throw new Exception("Something went wrong when creating a new Patients" + e.Message);
        }
    }

    public async Task<Patient?> GetPatientById(string id)
    {
        try
        {
            var patient = await _patientDbContext.PatientsTable!.FindAsync(id) ?? throw new KeyNotFoundException("No such patient found");
            return patient;

        }
        catch (Exception e)
        {
            throw new Exception("Something went wrong when deleting Patients with id: " +id + e.Message);
            
        }
    }

    public async Task Delete(string id)
    {
        try
        {
            var patient = await _patientDbContext.PatientsTable!.FindAsync(id) ?? throw new KeyNotFoundException("No such patient found");
            _patientDbContext.PatientsTable.Remove(patient);
            await _patientDbContext.SaveChangesAsync();
            
        }
        catch (Exception e)
        {
            throw new Exception("Something went wrong when deleting Patients with id: " +id + e.Message);
            
        }
    }

    public async Task<Patient?> Update(Patient patient)
    {
        try
        {
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
            throw new Exception("Something went wrong when updating Patients: "  + e.Message);
        }
    }

    
    public async Task<List<Patient>> GetAllPatients()
    {
        try
        {
            return await _patientDbContext.PatientsTable!.ToListAsync();
        }
        catch (Exception e)
        {
            throw new Exception("Something went wrong when getting a list of all Patients: "  + e.Message);
        }
    }
    
    public void RebuildDb()
    {
        try
        {
            _patientDbContext.Database.EnsureDeleted();
            _patientDbContext.Database.EnsureCreated();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }

    }
}