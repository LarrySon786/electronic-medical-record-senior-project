using ElectronicMedicalRecord.Database;
using ElectronicMedicalRecord.Services.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ElectronicMedicalRecord.Services.Database;

public class ResetDatabase
{
    private readonly DbContextFactoryHelper _context;

    public ResetDatabase(DbContextFactoryHelper context)
    {
        _context = context;
    }

    public async Task ResetDatabaseAsync(ProjectDatabaseConnection? context = null)
    {
        await _context.ExecuteAsync(async db =>
        {
            // Delete existing database
            await db.Database.EnsureDeletedAsync();

            // Recreate database using current migrations
            await db.Database.MigrateAsync();

            // SEED DATA


            // SAVE database changes
            await db.SaveChangesAsync();
        }, context);
    }
}