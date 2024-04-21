using DefaultNamespace;

namespace PatientService.DataAccess.Interfaces;

public interface IPatientRepository
{
    public Task<Patient?> Create(Patient patient);
    public Task Delete(int id);
    public Task<Patient?> Update(Patient patient);
    public Task<List<Patient>> GetAllPatients();
    public void RebuildDb();

}