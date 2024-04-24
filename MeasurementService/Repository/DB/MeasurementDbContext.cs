using DefaultNamespace;
using Microsoft.EntityFrameworkCore;

namespace MeasurementService;

public class MeasurementDbContext : DbContext
{
    public MeasurementDbContext(DbContextOptions<MeasurementDbContext> options)
        : base(options) {
    }

    public DbSet<Measurement>? MeasurementsTable { get; set; }
}