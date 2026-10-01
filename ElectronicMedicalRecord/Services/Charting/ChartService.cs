using System.ComponentModel.DataAnnotations;
using ElectronicMedicalRecord.Database;
using ElectronicMedicalRecord.Models;
using ElectronicMedicalRecord.Services.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ElectronicMedicalRecord.Services;

public class ChartService
{
    private readonly DbContextFactoryHelper _factory;

    public ChartService(DbContextFactoryHelper factory)
    {
        _factory = factory;
    }

    // GET all charts
    public async Task<List<Chart>> GetAllCharts(ProjectDatabaseConnection? context = null)
    {
        return await _factory.ExecuteAsync(async db =>
        {
            return await ChartQuery(db).ToListAsync();
        }, context);
    }

    // Get All Charts by Patient Id
    public async Task<List<Chart>> GetAllChartsByPatientId(int patientId, ProjectDatabaseConnection? context = null)
    {
        return await _factory.ExecuteAsync(async db =>
        {
            return await ChartQuery(db).Where(x => x.PatientId == patientId).ToListAsync();
        }, context);
    }

    // Get All Charts by Practitioner Id
    public async Task<List<Chart>> GetAllChartsByPractitionerId(int practitionerId, ProjectDatabaseConnection? context = null)
    {
        return await _factory.ExecuteAsync(async db =>
        {
            return await ChartQuery(db).Where(x => x.PractitionerId == practitionerId).ToListAsync();
        }, context);
    }


    // Get Chart by Id
    public async Task<Chart?> GetChartById(int chartId, ProjectDatabaseConnection? context = null)
    {
        return await _factory.ExecuteAsync(async db =>
        {
            return await ChartQuery(db).Where(x => x.Id == chartId).FirstOrDefaultAsync();
        }, context);
    }


    // Create Chart for Patient
    public async Task<Chart> CreateNewChart(int patientId, int practitionerId,ProjectDatabaseConnection? context = null)
    {
        return await _factory.ExecuteAsync(async db =>
        {
            var entity = new Chart()
            {
                PatientId = patientId,
                PractitionerId = practitionerId,
            };

            db.ChartDb.Add(entity);
            await db.SaveChangesAsync();
            return entity;
        }, context);
    }


    // Update Chart for Patient
    public async Task<Chart> UpdateChart(Chart chart, ProjectDatabaseConnection? context = null)
    {
        return await _factory.ExecuteAsync(async db =>
        {
            var errors = ValidateChart(chart);
            if (errors.Count() > 0)
            {
                string message = string.Empty;
                foreach(var err in errors)
                {
                    message += $" {err}";
                }
                throw new ValidationException($"Could not update chart. {message}");
            }

            var entity = await GetChartById(chart.Id, db);
            if (entity == null) throw new Exception("No chart found to update");

            entity.SubjectiveNotes = chart.SubjectiveNotes;
            entity.ObjectiveNotes = chart.ObjectiveNotes;
            entity.Assesment = chart.Assesment;
            entity.Plan = chart.Plan;

            await db.SaveChangesAsync();
            return entity;
        }, context);
    }

    // Server side validation
    private List<ValidationResult> ValidateChart(Chart chart, List<ValidationResult>? errors = null)
    {
        if (errors == null) errors = new();

        var validationContext = new ValidationContext(chart);
        Validator.TryValidateObject(chart, validationContext, errors, validateAllProperties: true);

        return errors;
    }

    // Standard query
    private IQueryable<Chart> ChartQuery(ProjectDatabaseConnection context)
    {
        return context.ChartDb
            .Include(x => x.Patient)
                .ThenInclude(x => x!.MedicalOverview)
                    .ThenInclude(x => x!.Medications)
            .Include(x => x.Practitioner);
    }

}