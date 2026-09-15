using ElectronicMedicalRecord.Components;
using ElectronicMedicalRecord.Database;
using ElectronicMedicalRecord.Services.Database;
using ElectronicMedicalRecord.Services.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// DATABASE set-up
builder.Services.AddDbContextFactory<ProjectDatabaseConnection>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Scoped
);

// SCOPED SERVICES
builder.Services.AddScoped<DbContextFactoryHelper>(); // This is a data base context builder helper service.
builder.Services.AddScoped<SeedData>(); // Allows methods to seed data for development
builder.Services.AddScoped<ResetDatabase>(); // Allows reseting database for developers
builder.Services.AddScoped<TestConnectionService>(); // A temporary service to test Database connections.




var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
