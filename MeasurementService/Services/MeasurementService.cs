using AutoMapper;
using DefaultNamespace;
using Newtonsoft.Json;
using OpenTelemetry.Trace;

namespace MeasurementService
{
    public class MeasurementService : IMeasurementService
    {
        private readonly IMeasurementRepo _measurementRepo;
        private readonly IMapper _mapper;
        private readonly Tracer _tracer;
        private readonly HttpClient _httpClient;
       

        public MeasurementService(IMeasurementRepo measurementRepo, IMapper mapper, Tracer tracer, HttpClient httpClient)
        {
            _measurementRepo = measurementRepo;
            _mapper = mapper;
            _tracer = tracer;
            _httpClient = httpClient;
        }

        public async Task<Measurement> GetMeasurementById(int id)
        {
            using var activity = _tracer.StartActiveSpan("GetMeasurementByIdInService");
            var measurement = await _measurementRepo.GetMeasurementById(id);
            return measurement;
        }

        public async Task<List<Measurement>> GetAllMeasurement(string ssn)
        {
            
            using var activity = _tracer.StartActiveSpan("GetAllMeasurementInService");
            
            var requestUrl = "GetById/"+ssn; 
            
                using var httpResponse = await _httpClient.GetAsync("http://dls-devops-PatientService-1:8081/Patient/" + requestUrl);
                
                if (httpResponse.IsSuccessStatusCode)
                {
                    var responseContent = await httpResponse.Content.ReadAsStringAsync();
                    var patient = JsonConvert.DeserializeObject<Patient>(responseContent);
                    
                    if (patient?.Ssn == ssn)
                    {
                        var measurements = await _measurementRepo.GetAllMeasurement(ssn);
                        return measurements;
                    }
                }
                Monitoring.Monitoring.Log.Error("Unable to GetAllMeasurement in service.");
                throw new Exception("No such patient with this ssn:" + ssn);
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
            _measurementRepo.RebuildDb();
        }
    }
}
