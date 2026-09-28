using System.Text.Json;
using System.Text.Json.Serialization;
using ElectronicMedicalRecord.Database;
using ElectronicMedicalRecord.Models;
using ElectronicMedicalRecord.Models.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ElectronicMedicalRecord.Services.Database;

public class EmployeeSeeder
{
    private readonly IDbContextFactory<ProjectDatabaseConnection> _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public EmployeeSeeder(IDbContextFactory<ProjectDatabaseConnection> context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // Responsible for seeding employee data
    public async Task SeedEmployeesAsync(ProjectDatabaseConnection context)
    {
        // Fetch Json file data
        var file = File.ReadAllText("JSON/EmployeeSeeder.json");
        var employeeDefinition = JsonSerializer.Deserialize<List<TestEmployeeUserDto>>(file, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() }
        });

        // Create each employee
        foreach (TestEmployeeUserDto user in employeeDefinition!)
        {
            // Create employee identity account
            ApplicationUser applicationUser = new()
            {
                UserName = user.UserName,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                Id = user.Id,
            };

            // Create User Role
            var userResult = await _userManager.CreateAsync(applicationUser, user.Password);
            if (!userResult.Succeeded) throw new Exception("Could not create application user.");

            // Assign Roles
            var roleResult = await _userManager.AddToRoleAsync(applicationUser, user.Role);
            if (!roleResult.Succeeded) throw new Exception("Could not assign user role");

            // Create employee data
            TestEmployeeModel entity = new()
            {
                FirstName = user.TestEmployeeModel.FirstName,
                applicationUserId = user.Id,
            };

            // Create employee
            context.TestEmployeeModelDb.Add(entity);
        }
        await context.SaveChangesAsync();
    }
}