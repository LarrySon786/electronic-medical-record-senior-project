
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



    // Database Model Builder
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // Builds Model relationships between tables

    }

}