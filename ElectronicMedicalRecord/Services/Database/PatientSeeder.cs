using System.Text.Json;
using System.Text.Json.Serialization;
using ElectronicMedicalRecord.Database;
using ElectronicMedicalRecord.Models;
using Microsoft.EntityFrameworkCore;

namespace ElectronicMedicalRecord.Services.Database;

public class PatientSeeder
{
    private readonly IDbContextFactory<ProjectDatabaseConnection> _context;
    private readonly PatientService _patientService;

    public PatientSeeder(IDbContextFactory<ProjectDatabaseConnection> context, PatientService patientService)
    {
        _context = context;
        _patientService = patientService;
    }

    // Responsible for seeding patient data
    public async Task SeedPatientsAsync(ProjectDatabaseConnection context)
    {
        // Fetch Json file data
        var file = File.ReadAllText("JSON/PatientSeed.json");
        var patientDefinition = JsonSerializer.Deserialize<List<Patient>>(file, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() }
        });

        // Create each patient
        foreach (Patient patient in patientDefinition!)
        {
            await _patientService.CreatePatient(patient, context);
        }
    }
}

