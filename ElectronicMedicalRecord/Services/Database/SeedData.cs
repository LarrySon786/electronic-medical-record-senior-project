using System.Text.Json;
using System.Text.Json.Serialization;
using ElectronicMedicalRecord.Database;
using ElectronicMedicalRecord.Models;
using Microsoft.EntityFrameworkCore;

namespace ElectronicMedicalRecord.Services.Database;

public class SeedData
{
    private readonly IDbContextFactory<ProjectDatabaseConnection> _context;
    private readonly PatientSeeder _patientSeeder;

    public SeedData(IDbContextFactory<ProjectDatabaseConnection> context, PatientSeeder patientSeeder)
    {
        _context = context;
        _patientSeeder = patientSeeder;
    }

    // Responsible for seeding data in the application.
    public async Task SeedAsync(ProjectDatabaseConnection context)
    {
        // Responsible for seeding patient data
        await _patientSeeder.SeedPatientsAsync(context);
    
    }
}

