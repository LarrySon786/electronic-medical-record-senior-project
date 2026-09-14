// *****************************
// This file creates a disposable Db Context for every function that queries the database for the app.
// Often times database queries struggle if previous contexts are not closed in blazor.
// This DbContextFactory makes a new context and then disposes it after each function operation.
// *****************************

using ElectronicMedicalRecord.Database;
using Microsoft.EntityFrameworkCore;

namespace ElectronicMedicalRecord.Services.Extensions;

public class DbContextFactoryHelper
{
    private readonly IDbContextFactory<ProjectDatabaseConnection> _context;

    public DbContextFactoryHelper(IDbContextFactory<ProjectDatabaseConnection> context)
    {
        _context = context;
    }

    // The two functions below are responsible for creating and then disposing every Db context connection at the beginning
    // of and at the end of each method called. So if a use calls GetPatientById()... these helpers create a DbContext,
    // perform the operation, and then dispose that DbContext. This way the DbContext does NOT exist through the entire
    // life line of the application and create problems later on.

    public async Task<T> ExecuteAsync<T>(Func<ProjectDatabaseConnection, Task<T>> operation, ProjectDatabaseConnection? existing = null)
    {
        if (existing != null)
        {
            return await operation(existing);
        }

        await using var context = await _context.CreateDbContextAsync();
        return await operation(context);
    }

    public async Task ExecuteAsync(Func<ProjectDatabaseConnection, Task> operation, ProjectDatabaseConnection? existing = null)
    {
        if (existing != null)
        {
            await operation(existing);
            return;
        }

        await using var context = await _context.CreateDbContextAsync();
        await operation(context);
    }

}

