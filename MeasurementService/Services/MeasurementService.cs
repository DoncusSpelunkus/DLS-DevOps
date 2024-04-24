
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

        public async Task<Measurement> GetMeasurementById(int id, string ssn)
        {
            
            if (!await IsPatientSsnCorrect(id, ssn))
            {
                throw new ArgumentException("Invalid SSN provided for the measurement.");
            }
            
            return await _measurementRepo.GetMeasurementById(id);
        }

        public async Task<List<Measurement>> GetAllMeasurement(string ssn)
        {
            if (!await IsPatientSsnCorrect(ssn))
            {
                throw new ArgumentException("Invalid SSN provided for the measurements.");
            }
       
            return await _measurementRepo.GetAllMeasurement();
        }

        private async Task<bool> IsPatientSsnCorrect(int id, string ssn)
        {
          
            var measurement = await _measurementRepo.GetMeasurementById(id);
            return measurement != null && measurement.PatientSsn == ssn;
        }

        private async Task<bool> IsPatientSsnCorrect(string ssn)
        {
            var measurements = await _measurementRepo.GetMeasurementsBySsn(ssn);
            return measurements is { Count: > 0 };
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