using DefaultNamespace;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Trace;

namespace MeasurementService
{
    public class MeasurementRepo : IMeasurementRepo
    {
        private readonly MeasurementDbContext _measurementDbContext;
        private readonly Tracer _tracer;
        
        public MeasurementRepo(MeasurementDbContext measurementDbContext, Tracer tracer)
        {
            _measurementDbContext = measurementDbContext;
            _tracer = tracer;
        }

       public async Task<Measurement?> GetMeasurementById(int id)
        {
            try
            {
                using var activity = _tracer.StartActiveSpan("GetMeasurementByIdInRepo");
                var measurement =  await _measurementDbContext.MeasurementsTable.FindAsync(id);
                if (measurement != null)
                {
                    measurement.Seen = true;
                    await _measurementDbContext.SaveChangesAsync();
                }
                return measurement;
            }
            catch (Exception e)
            {
                Monitoring.Monitoring.Log.Error("Unable to GetMeasurementById in repo.");
                throw new Exception("Something went wrong when getting measurement by ID: " + e.Message);
            }
        }


        public async Task<List<Measurement>> GetAllMeasurement(string ssn)
        {
            try
            {
                using var activity = _tracer.StartActiveSpan("GetAllMeasurementInRepo");
                var measurements = await _measurementDbContext.MeasurementsTable.Where(m => m.PatientSsn == ssn).ToListAsync();

                foreach (var measurement in measurements)
                {
                    measurement.Seen = true;
                }

                await _measurementDbContext.SaveChangesAsync();

                return measurements;
            }
            catch (Exception e)
            {
                Monitoring.Monitoring.Log.Error("Unable to GetAllMeasurement in repo.");
                throw new Exception("Something went wrong when getting all measurements: " + e.Message);
            }
        }


        public async Task<Measurement?> CreateMeasurement(Measurement measurement)
        {
            try
            {
                using var activity = _tracer.StartActiveSpan("CreateMeasurementInRepo");
                await _measurementDbContext.MeasurementsTable.AddAsync(measurement);
                await _measurementDbContext.SaveChangesAsync();
                return measurement;
            }
            catch (Exception e)
            {
                Monitoring.Monitoring.Log.Error("Unable to CreateMeasurement in repo.");
                throw new Exception("Something went wrong when creating a new measurement: " + e.Message);
            }
        }

        public async Task DeleteMeasurement(int id)
        {
            try
            {
                using var activity = _tracer.StartActiveSpan("DeleteMeasurementInRepo");
                var measurement = await _measurementDbContext.MeasurementsTable.FindAsync(id) ?? throw new KeyNotFoundException("No such measurement found");
                _measurementDbContext.MeasurementsTable.Remove(measurement);
                await _measurementDbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Monitoring.Monitoring.Log.Error("Unable to DeleteMeasurementInRepo.");
                throw new Exception("Something went wrong when deleting measurement with ID: " + id + ". " + e.Message);
            }
        }

        public async Task<Measurement?> UpdateMeasurement(Measurement measurement)
        {
            try
            {
                using var activity = _tracer.StartActiveSpan("UpdateMeasurementInRepo");
                var existingMeasurement = await _measurementDbContext.MeasurementsTable.FindAsync(measurement.Id);
                if (existingMeasurement == null)
                {
                    throw new ArgumentException("Measurement not found", nameof(measurement));
                }
                
                existingMeasurement.Date = measurement.Date;
                existingMeasurement.Systolic = measurement.Systolic;
                existingMeasurement.Diastolic = measurement.Diastolic;

                await _measurementDbContext.SaveChangesAsync();
                return existingMeasurement;
            }
            catch (Exception e)
            {
                Monitoring.Monitoring.Log.Error("Unable to UpdateMeasurement in repo.");
                throw new Exception("Something went wrong when updating measurement: " + e.Message);
            }
        }


        public void RebuildDb()
        {
            try
            {
                _measurementDbContext.Database.EnsureDeleted();
                _measurementDbContext.Database.EnsureCreated();
            }
            catch (Exception e)
            {
                throw new Exception("Something went wrong when rebuilding the database: " + e.Message);
            }
        }
    }
}
