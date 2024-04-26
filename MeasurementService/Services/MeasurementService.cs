using AutoMapper;
using DefaultNamespace;
using OpenTelemetry.Trace;

namespace MeasurementService
{
    public class MeasurementService : IMeasurementService
    {
        private readonly IMeasurementRepo _measurementRepo;
        private readonly IMapper _mapper;
        private readonly Tracer _tracer;

        public MeasurementService(IMeasurementRepo measurementRepo, IMapper mapper, Tracer tracer)
        {
            _measurementRepo = measurementRepo;
            _mapper = mapper;
            _tracer = tracer;
        }

        public async Task<Measurement?> GetMeasurementById(int id)
        {
            using var activity = _tracer.StartActiveSpan("GetMeasurementByIdInService");
            var measurement = await _measurementRepo.GetMeasurementById(id);
            return measurement;
        }

        public async Task<List<Measurement>> GetAllMeasurement(string ssn)
        {
            using var activity = _tracer.StartActiveSpan("GetAllMeasurementInService");
            var measurements = await _measurementRepo.GetAllMeasurement(ssn);
            return measurements;
        }

        public async Task<Measurement?> CreateMeasurement(MeasurementDto measurementDto)
        {
            using var activity = _tracer.StartActiveSpan("CreateMeasurementInService");
            var measurement = _mapper.Map<Measurement>(measurementDto);
            measurement.Date = DateTime.Now;
            var createdMeasurement = await _measurementRepo.CreateMeasurement(measurement);
            return createdMeasurement;
        }

        public async Task DeleteMeasurement(int id)
        {
            using var activity = _tracer.StartActiveSpan("DeleteMeasurementInService");
            await _measurementRepo.DeleteMeasurement(id);
  
        }

        public async Task<Measurement?> UpdateMeasurement(MeasurementDto measurementDto, int id)
        {
            using var activity = _tracer.StartActiveSpan("UpdateMeasurementInService");
            var currentMeasurement = await _measurementRepo.GetMeasurementById(id);
            currentMeasurement.Diastolic = measurementDto.Diastolic;
            currentMeasurement.Systolic = measurementDto.Systolic;
            await _measurementRepo.UpdateMeasurement(currentMeasurement);
            return currentMeasurement;
        }

        public void RebuildDb()
        {
            using var activity = _tracer.StartActiveSpan("RebuildDbInService");
            _measurementRepo.RebuildDb();
        }
    }
}
