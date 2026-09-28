using System.Security.Claims;
using ElectronicMedicalRecord.Database;
using ElectronicMedicalRecord.Models;
using ElectronicMedicalRecord.Services.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ElectronicMedicalRecord.Services;

public class AuthService
{
    private readonly IHttpContextAccessor _httpAccessor;
    private readonly DbContextFactoryHelper _factory;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AuthService(IHttpContextAccessor httpAccessor, DbContextFactoryHelper factory, SignInManager<ApplicationUser> signInManager)
    {
        _httpAccessor = httpAccessor;
        _factory = factory;
        _signInManager = signInManager;
    }

    // Get Current User Async | Will return employee model once it is added } Using test model temporarily
    public async Task<TestEmployeeModel?> GetCurrentUserAsync(ProjectDatabaseConnection? context = null)
    {
        return await _factory.ExecuteAsync(async db =>
        {
            if (_httpAccessor.HttpContext == null) throw new Exception("No Http Context found.");
            var identity = _httpAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier); // Primary Key is the ID

            if (identity == null) return null;

            return await db.TestEmployeeModelDb
                .Include(x => x.applicationUser)
                .FirstOrDefaultAsync(x => x.applicationUser!.Id == identity);
        }, context);
    }


}