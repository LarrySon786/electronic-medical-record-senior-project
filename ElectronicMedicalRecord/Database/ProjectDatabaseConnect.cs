
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ElectronicMedicalRecord.Models;

namespace ElectronicMedicalRecord.Database;


public class ProjectDatabaseConnection : IdentityDbContext<ApplicationUser>
{
    public ProjectDatabaseConnection(DbContextOptions<ProjectDatabaseConnection> options) : base(options)
    {
    }

    // Database Tables
    public DbSet<TestConnection> TestConnectionDb{ get; set; } // Table in database to test that database settings are correct
    public DbSet<Patient> PatientDb { get; set; }
    public DbSet<Medical> MedicalOverviewDb { get; set; }
    public DbSet<Medication> MedicationDb { get; set; }


    // Database Model Builder
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Builds Model relationships between tables
        builder.Entity<Patient>()
            .HasOne(x => x.MedicalOverview)
            .WithOne(x => x.Patient)
            .HasForeignKey<Medical>(x => x.PatientId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Medical>()
            .HasMany(x => x.Medications)
            .WithOne(x => x.Medical)
            .HasForeignKey(x => x.MedicalId)
            .OnDelete(DeleteBehavior.Cascade);

    }

}