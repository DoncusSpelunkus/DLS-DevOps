using DefaultNamespace;
using Microsoft.EntityFrameworkCore;


public class PatientDbContext : DbContext
{
    public PatientDbContext(DbContextOptions<PatientDbContext> options)
        : base(options) {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {   
        // setting email as unique
        modelBuilder.Entity<Patient>()
            .HasIndex(u => u.Mail)
            .IsUnique();
        // setting the ssn as the primary key
        modelBuilder.Entity<Patient>().HasKey(p => p.Ssn);
        // setting ssn as unique, may not need this as we also set it as primary key just above
        modelBuilder.Entity<Patient>()
            .HasIndex(u => u.Ssn)
            .IsUnique();
      
    }

    public DbSet<Patient>? PatientsTable { get; set; }
}
