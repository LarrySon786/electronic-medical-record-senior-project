using ElectronicMedicalRecord.Database;
using ElectronicMedicalRecord.Models;
using ElectronicMedicalRecord.Services.Extensions;
using Microsoft.EntityFrameworkCore;

// ****************
// This file is simply to affirm that the program correctly makes database queries
// ****************


namespace ElectronicMedicalRecord.Services.Database;

public class TestConnectionService
{
    private readonly DbContextFactoryHelper _contextFactory;

    public TestConnectionService(DbContextFactoryHelper context)
    {
        _contextFactory = context;
    }

    // GET all test connection entities
    public async Task<List<TestConnection>> GetAllTestConnections(ProjectDatabaseConnection? context = null)
    {
        return await _contextFactory.ExecuteAsync(async db =>
        {
            return await db.TestConnectionDb.ToListAsync();
        }, context);
    }


    // Create new test connection entity
    public async Task<TestConnection> CreateTestConnectionEntity(TestConnection newConnection, ProjectDatabaseConnection? context = null)
    {
        return await _contextFactory.ExecuteAsync(async db =>
        {
            db.TestConnectionDb.Add(newConnection);
            await db.SaveChangesAsync();
            return newConnection;
        }, context);
    }




}