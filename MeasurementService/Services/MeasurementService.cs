
using AutoMapper;
using DefaultNamespace;


namespace MeasurementService
{
    public class MeasurementService : IMeasurementService
    {
        private readonly IMeasurementRepo _measurementRepo; 
        private readonly IMapper _mapper;

        public MeasurementService(IMeasurementRepo measurementRepo, IMapper mapper)
        {
            _measurementRepo = measurementRepo;
            _mapper = mapper;
        }

        public async Task<Measurement> GetMeasurementById(int id)
        {
            return await _measurementRepo.GetMeasurementById(id);
        }

        public async Task<List<Measurement>> GetAllMeasurement(string ssn)
        {
            return await _measurementRepo.GetAllMeasurement(ssn);
        }
        
        public Task<Measurement?> CreateMeasurement(MeasurementDto measurementDto)
        {
            
            var measurement = _mapper.Map<Measurement>(measurementDto);
            measurement.Date = DateTime.Now;
            
            return _measurementRepo.CreateMeasurement(measurement);
        }

        public Task DeleteMeasurement(int id)
        {
            return _measurementRepo.DeleteMeasurement(id);
        }

        public async Task<Measurement?> UpdateMeasurement(MeasurementDto measurementDto, int id)
        {
            var currentMeasurement = await _measurementRepo.GetMeasurementById(id);
            currentMeasurement.Diastolic = measurementDto.Diastolic;
            currentMeasurement.Systolic = measurementDto.Systolic;
            await _measurementRepo.UpdateMeasurement(currentMeasurement);
            return currentMeasurement;
        }
        
        public void RebuildDb()
        {
            _measurementRepo.RebuildDb();
        }
        
        
    }
}