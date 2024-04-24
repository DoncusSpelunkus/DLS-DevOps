using DefaultNamespace;

namespace MeasurementService;

public interface IMeasurementService
{
    public Task<Measurement> GetMeasurementById(int id, string ssn);
    public Task<List<Measurement>> GetAllMeasurement(string ssn);
    public Task<Measurement?> CreateMeasurement(Measurement measurement);
    public Task DeleteMeasurement(int id);
    public Task<Measurement?> UpdateMeasurement(Measurement measurement);
    public void RebuildDb();
}