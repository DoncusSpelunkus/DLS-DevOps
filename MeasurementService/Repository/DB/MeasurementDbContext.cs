using DefaultNamespace;
using Microsoft.EntityFrameworkCore;

namespace MeasurementService.Repository.DB;

public class MeasurementDbContext : DbContext
{
    public MeasurementDbContext(DbContextOptions<MeasurementDbContext> options)
        : base(options) {
    }

    public DbSet<Measurement>? MeasurementsTable { get; set; }
}