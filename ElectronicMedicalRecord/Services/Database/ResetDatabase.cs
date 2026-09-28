using ElectronicMedicalRecord.Database;
using ElectronicMedicalRecord.Services.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ElectronicMedicalRecord.Services.Database;

public class ResetDatabase
{
    private readonly DbContextFactoryHelper _context;
    private readonly SeedData _seeder;

    public ResetDatabase(DbContextFactoryHelper context, SeedData seeder)
    {
        _context = context;
        _seeder = seeder;
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
            await _seeder.SeedAsync(db);

            // SAVE database changes
            await db.SaveChangesAsync();
        }, context);
    }
}