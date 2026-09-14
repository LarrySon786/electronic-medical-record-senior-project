using ElectronicMedicalRecord.Database;
using Microsoft.EntityFrameworkCore;

namespace ElectronicMedicalRecord.Services.Database;

public class SeedData
{
    private readonly IDbContextFactory<ProjectDatabaseConnection> _context;

    public SeedData(IDbContextFactory<ProjectDatabaseConnection> context)
    {
        _context = context;
    }


    


}