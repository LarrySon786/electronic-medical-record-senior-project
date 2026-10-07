using System.Text.Json;
using System.Text.Json.Serialization;
using ElectronicMedicalRecord.Database;
using ElectronicMedicalRecord.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace ElectronicMedicalRecord.Services.Database;

public class EmployeeSeeder
{
    private readonly EmployeeService _employeeService;

    public EmployeeSeeder(EmployeeService employeeService)
    {

        _employeeService = employeeService;
    }

    // Responsible for seeding employee data
    public async Task SeedEmployeesAsync(ProjectDatabaseConnection context)
    {
        // Avoid duplicates: do nothing if chats already exist in the database
        if (await context.EmployeeDb.AnyAsync())
            return;

        // Fetch Json file data
        var file = File.ReadAllText("JSON/EmployeeSeeder.json");
        var employeeDefinition = JsonSerializer.Deserialize<List<CreateEmployeeDto>>(file, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() }
        });

        // Create each employee
        foreach (CreateEmployeeDto user in employeeDefinition!)
        {
            // Create employee account
            await _employeeService.CreateEmployeeByDtoAsync(user, context);
        }
        await context.SaveChangesAsync();
    }
}