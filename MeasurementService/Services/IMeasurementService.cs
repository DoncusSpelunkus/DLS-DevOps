using DefaultNamespace;

namespace MeasurementService;

public interface IMeasurementService
{
    public Task<Measurement> GetMeasurementById(int id);
    public Task<List<Measurement>> GetAllMeasurement(string ssn);
    public Task<Measurement?> CreateMeasurement(MeasurementDto measurementDto);
    public Task DeleteMeasurement(int id);
    public Task<Measurement?> UpdateMeasurement(MeasurementDto measurementDto, int id);
    public void RebuildDb();
}