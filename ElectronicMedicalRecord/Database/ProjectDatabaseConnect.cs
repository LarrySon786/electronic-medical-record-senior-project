
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
    public DbSet<TestConnection> TestConnectionDb { get; set; } // Table in database to test that database settings are correct
    public DbSet<Patient> PatientDb { get; set; }
    public DbSet<Medical> MedicalOverviewDb { get; set; }
    public DbSet<Medication> MedicationDb { get; set; }
    public DbSet<Employee> EmployeeDb { get; set; }
    public DbSet<ApplicationUser> ApplicationUserDb { get; set; } // The identity table for users
    public DbSet<Message> MessageDb { get; set; }
    public DbSet<Chat> ChatDb { get; set; }
    public DbSet<Chart> ChartDb { get; set; } // Database table for charting by practitioners

    // Database Model Builder
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);  // Builds Model relationships between tables

        // Patients
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

        builder.Entity<Employee>()
            .HasOne(x => x.ApplicationUser)
            .WithOne(x => x.Employee)
            .HasForeignKey<Employee>(x => x.ApplicationUserId);

        builder.Entity<Message>()
            .HasOne(x => x.Chat)
            .WithMany(x => x.Messages)
            .HasForeignKey(x => x.ChatId);

        builder.Entity<Message>()
            .HasOne(x => x.Employee)
            .WithOne()
            .HasForeignKey<Message>(x => x.EmployeeId);
        // Charts
        builder.Entity<Chart>()
            .HasOne(x => x.Practitioner)
            .WithMany()
            .HasForeignKey(x => x.PractitionerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Chart>()
            .HasOne(x => x.Patient)
            .WithMany()
            .HasForeignKey(x => x.PatientId)
            .OnDelete(DeleteBehavior.Restrict);
    }

}