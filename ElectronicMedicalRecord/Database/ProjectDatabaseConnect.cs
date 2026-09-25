
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ElectronicMedicalRecord.Database.Models.Authentication;
using ElectronicMedicalRecord.Database.Models;

namespace ElectronicMedicalRecord.Database;


public class ProjectDatabaseConnection : IdentityDbContext<ApplicationUser>
{
    public ProjectDatabaseConnection(DbContextOptions<ProjectDatabaseConnection> options) : base(options)
    {
    }

    // Database Tables
    public DbSet<TestConnection> TestConnectionDb{ get; set; } // Table in database to test that database settings are correct
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Medical> MedicalRecords { get; set; }
    public DbSet<Medication> Medications { get; set; }



    // Database Model Builder
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // Builds Model relationships between tables
        builder.Entity<Medical>()
            .HasOne(medical => medical.Patient)
            .WithOne()
            .HasForeignKey<Medical>(medical => medical.PatientId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Medication>()
            .HasOne(medication => medication.Medical)
            .WithMany(medical => medical.Medications)
            .HasForeignKey(medication => medication.MedicalId)
            .OnDelete(DeleteBehavior.Cascade);

    }

}