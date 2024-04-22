using DefaultNamespace;

namespace PatientService.DataAccess.Interfaces;

public interface IPatientRepository
{
    public Task<Patient?> Create(Patient patient);
    public Task<Patient?> GetPatientById(string id);
    public Task Delete(string id);
    public Task<Patient?> Update(Patient patient);
    public Task<List<Patient>> GetAllPatients();
    public void RebuildDb();

}