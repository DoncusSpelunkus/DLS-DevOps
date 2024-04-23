
using DefaultNamespace;


namespace MeasurementService
{
    public class MeasurementService : IMeasurementService
    {
        private readonly IMeasurementRepo _measurementRepo; 

        public MeasurementService(IMeasurementRepo measurementRepo)
        {
            _measurementRepo = measurementRepo;
        }

        public Task<Measurement> GetMeasurementById(int id)
        {
            return _measurementRepo.GetMeasurementById(id);
        }

        public Task<List<Measurement>> GetAllMeasurement()
        {
            return _measurementRepo.GetAllMeasurement();
        }

        public Task<Measurement?> CreateMeasurement(Measurement measurement)
        {
            return _measurementRepo.CreateMeasurement(measurement);
        }

        public Task DeleteMeasurement(int id)
        {
            return _measurementRepo.DeleteMeasurement(id);
        }

        public Task<Measurement?> UpdateMeasurement(Measurement measurement)
        {
            return _measurementRepo.UpdateMeasurement(measurement);
        }

        public void RebuildDb()
        {
            _measurementRepo.RebuildDb();
        }
    }
}