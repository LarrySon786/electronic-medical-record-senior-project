using ElectronicMedicalRecord.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ElectronicMedicalRecord.Services.Database;

public class RoleSeeder
{
    private readonly IDbContextFactory<ProjectDatabaseConnection> _context;
    private readonly RoleManager<IdentityRole> _roleManager;

    public RoleSeeder(IDbContextFactory<ProjectDatabaseConnection> context, RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _roleManager = roleManager;
    }

    // Responsible for seeding User Roles data
    public async Task SeedRolesAsync(ProjectDatabaseConnection context)
    {
        // If a role does NOT exist, then add it.
        if (!await _roleManager.RoleExistsAsync("Admin"))
            await _roleManager.CreateAsync(new IdentityRole("Admin"));

        if (!await _roleManager.RoleExistsAsync("Practitioner"))
            await _roleManager.CreateAsync(new IdentityRole("Practitioner"));

        if (!await _roleManager.RoleExistsAsync("Employee"))
            await _roleManager.CreateAsync(new IdentityRole("Employee"));
    }
}

