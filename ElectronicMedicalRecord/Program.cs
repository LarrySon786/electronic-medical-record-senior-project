using ElectronicMedicalRecord.Components;
using ElectronicMedicalRecord.Database;
using ElectronicMedicalRecord.Hubs;
using ElectronicMedicalRecord.Models;
using ElectronicMedicalRecord.Services;
using ElectronicMedicalRecord.Services.Database;
using ElectronicMedicalRecord.Services.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// DATABASE set-up
builder.Services.AddDbContextFactory<ProjectDatabaseConnection>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Scoped
);


// AUTHENTICATION and Authorization | ASP.NET Core Identity set-up
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthorization();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>() // Identity Core configurations
    .AddEntityFrameworkStores<ProjectDatabaseConnection>()
    .AddDefaultTokenProviders();

// Authentication and Authorization COOKIE set-up
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "EMR.Identity";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.SlidingExpiration = true;
    options.LoginPath = "/";
    options.Cookie.SameSite = SameSiteMode.Lax;
});


// SCOPED SERVICES
builder.Services.AddScoped<DbContextFactoryHelper>(); // This is a data base context builder helper service.
builder.Services.AddScoped<AuthService>(); // Authenticaion and Authorization actions
builder.Services.AddControllers(); // This establishes controllers for the app
builder.Services.AddScoped<PatientService>(); // Allows operations with patient data / objects. Included CRUD operations
builder.Services.AddScoped<EmployeeService>(); // Allows operations with employee data.
builder.Services.AddScoped<MessageService>(); // Allows operations with chat.
builder.Services.AddSignalR(); // SIGNALR: real-time communication used by the chat (ChatHub)
builder.Services.AddHttpContextAccessor(); // Gives access to the current HTTP request (e.g. the user's cookie).
builder.Services.AddScoped<ChartService>(); // Allows charting operations by practitioners and employees
builder.Services.AddScoped<ScheduleService>(); // Allows scheduling and appointment operations

// SCOPED Seed Services
builder.Services.AddScoped<TestConnectionService>(); // A temporary service to test Database connections.
builder.Services.AddScoped<SeedData>(); // Allows methods to seed data for development
builder.Services.AddScoped<ResetDatabase>(); // Allows reseting database for developers
builder.Services.AddScoped<PatientSeeder>(); // Seeds patient data
builder.Services.AddScoped<RoleSeeder>(); // Seeds role data
builder.Services.AddScoped<EmployeeSeeder>(); // Seeds employee data
builder.Services.AddScoped<ChatSeeder>(); // Seeds chat data
builder.Services.AddScoped<CookieProvider>(); //Service to get cookie value

var app = builder.Build();


// This feature automatically resets the database each time the app starts:
// using (var scope = app.Services.CreateScope())
// {
//     var seedData = scope.ServiceProvider.GetRequiredService<SeedData>();
//     var context = scope.ServiceProvider
//         .GetRequiredService<IDbContextFactory<ProjectDatabaseConnection>>()
//         .CreateDbContext();

//     await seedData.SeedAsync(context);
// }


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAuthentication(); // Allows authentication operations
app.UseAuthorization(); // Allows authorization operations 

app.UseAntiforgery();

app.MapControllers(); // Allows controller routes to be hit
app.MapHub<ChatHub>("/chathub"); //SignalR chat hub, reachable at /chathub


app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
