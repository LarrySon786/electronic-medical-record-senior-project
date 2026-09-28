using ElectronicMedicalRecord.Database;
using ElectronicMedicalRecord.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElectronicMedicalRecord.Controllers;

// This file still needs to add the employee service functionality once that is made

[ApiController]
[Route("account")]
public class LoginController : ControllerBase
{
    private readonly IDbContextFactory<ProjectDatabaseConnection> _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public LoginController(IDbContextFactory<ProjectDatabaseConnection> context,
        UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpPost("login")]
    public async Task<IResult> Login()
    {
        // Collect form input
        var form = await Request.ReadFormAsync();
        var email = form["Email"].ToString();
        var password = form["Password"].ToString();

        // Detect null fields
        if (string.IsNullOrEmpty(email)) return Results.Redirect("/?error=InvalidLogin");
        if (string.IsNullOrEmpty(password)) return Results.Redirect("/?error=InvalidLogin");

        // Verify user disabled / enabled status
        // var user = await employeeService.GetEmployeeByEmail(email); // Verify user exists to verify if disabled
        // if (user == null || user.IsDisabled == true || user.IdentityUser == null) return Results.Redirect("/login?error=true");

        // Sign user in
        var result = await _signInManager.PasswordSignInAsync(email, password, true, false); // Sign-in User
        if (!result.Succeeded) return Results.Redirect("/?error=InvalidLogin");

        // Redirect to app if successful login
        else return Results.Redirect("/patients");
    }

    [HttpPost("logout")]
    public async Task<IResult> Logout()
    {
        try
        {
            await _signInManager.SignOutAsync();
            return Results.Redirect("/?logout=true");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not logout. Ex: {ex}");
            return Results.StatusCode(500);
        }
    }

}