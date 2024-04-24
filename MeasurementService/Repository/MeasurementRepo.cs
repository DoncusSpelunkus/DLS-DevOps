using DefaultNamespace;
using Microsoft.EntityFrameworkCore;

namespace MeasurementService
{
    public class MeasurementRepo : IMeasurementRepo
    {
        private readonly MeasurementDbContext _measurementDbContext;
        
        public MeasurementRepo(MeasurementDbContext measurementDbContext)
        {
            _measurementDbContext = measurementDbContext;
        }

        public async Task<Measurement?> GetMeasurementById(int id)
        {
            try
            {
                var measurement = await _measurementDbContext.MeasurementsTable.FindAsync(id);
                if (measurement != null)
                {
                    measurement.Seen = true;
                    await _measurementDbContext.SaveChangesAsync();
                }
                return measurement;
            }
            catch (Exception e)
            {
                throw new Exception("Something went wrong when getting measurement by ID: " + e.Message);
            }
        }

        public async Task<List<Measurement>> GetAllMeasurement()
        {
            try
            {
                var measurements = await _measurementDbContext.MeasurementsTable.ToListAsync();
                foreach (var measurement in measurements)
                {
                    measurement.Seen = true;
                }
                await _measurementDbContext.SaveChangesAsync();
                return measurements;
            }
            catch (Exception e)
            {
                throw new Exception("Something went wrong when getting all measurements: " + e.Message);
            }
        }

        public async Task<Measurement?> CreateMeasurement(Measurement measurement)
        {
            try
            {
                await _measurementDbContext.MeasurementsTable.AddAsync(measurement);
                await _measurementDbContext.SaveChangesAsync();
                return measurement;
            }
            catch (Exception e)
            {
                throw new Exception("Something went wrong when creating a new measurement: " + e.Message);
            }
        }

        public async Task DeleteMeasurement(int id)
        {
            try
            {
                var measurement = await _measurementDbContext.MeasurementsTable.FindAsync(id) ?? throw new KeyNotFoundException("No such measurement found");
                _measurementDbContext.MeasurementsTable.Remove(measurement);
                await _measurementDbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception("Something went wrong when deleting measurement with ID: " + id + ". " + e.Message);
            }
        }

        public async Task<Measurement?> UpdateMeasurement(Measurement measurement)
        {
            try
            {
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

        public async Task<List<Measurement>> GetMeasurementsBySsn(string ssn)
        {
            return await _measurementDbContext.MeasurementsTable
                .Where(m => m.PatientSsn == ssn)
                .ToListAsync();
        }
    }
}
