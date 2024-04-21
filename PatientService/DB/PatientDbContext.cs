using DefaultNamespace;
using Microsoft.EntityFrameworkCore;


public class PatientDbContext : DbContext
{
    public PatientDbContext(DbContextOptions<PatientDbContext> options)
        : base(options) {
    }

    public DbSet<Patient>? PatientsTable { get; set; }
}
