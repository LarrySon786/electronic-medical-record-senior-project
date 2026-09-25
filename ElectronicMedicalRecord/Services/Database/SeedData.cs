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
    private readonly RoleSeeder _roleSeeder;
    private readonly EmployeeSeeder _employeeSeeder;

    public SeedData(IDbContextFactory<ProjectDatabaseConnection> context, PatientSeeder patientSeeder,
        EmployeeSeeder employeeSeeder, RoleSeeder roleSeeder)
    {
        _context = context;
        _patientSeeder = patientSeeder;
        _roleSeeder = roleSeeder;
        _employeeSeeder = employeeSeeder;
    }

    // Responsible for seeding data in the application.
    public async Task SeedAsync(ProjectDatabaseConnection context)
    {
        // Responsible for seeding patient data
        await _patientSeeder.SeedPatientsAsync(context);

        // Seed Roles
        await _roleSeeder.SeedRolesAsync(context);

        // Seed Employees (users)
        await _employeeSeeder.SeedEmployeesAsync(context);
    
    }
}

