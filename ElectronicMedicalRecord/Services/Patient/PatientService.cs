using System.ComponentModel.DataAnnotations;
using ElectronicMedicalRecord.Components.Pages;
using ElectronicMedicalRecord.Database;
using ElectronicMedicalRecord.Models;
using ElectronicMedicalRecord.Models.Dtos;
using ElectronicMedicalRecord.Services.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ElectronicMedicalRecord.Services;

public class PatientService
{
    private readonly DbContextFactoryHelper _context;

    public PatientService(DbContextFactoryHelper context)
    {
        _context = context;
    }

    // GET ALL patients
    public async Task<List<Patient>> GetAllPatients(ProjectDatabaseConnection? context = null)
    {
        return await _context.ExecuteAsync(async db =>
        {
            // Return a list of patients in the datbase (see query method for details returned)
            return await PatientQuery(db)
                .ToListAsync();
        }, context);
    }

    // GET patient by ID
    // Return a single patient (or null value) based on Id
    public async Task<Patient?> GetPatientById(int id, ProjectDatabaseConnection? context = null)
    {
        return await _context.ExecuteAsync(async db =>
        {
            return await PatientQuery(db)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }, context);
    }


    // GET patient by EMAIL
    // Return a single patient (or null value) based on Email
    public async Task<Patient?> GetPatientByEmail(string email, ProjectDatabaseConnection? context = null)
    {
        return await _context.ExecuteAsync(async db =>
        {
            return await PatientQuery(db)
                .Where(x => x.Email == email)
                .FirstOrDefaultAsync();
        }, context);
    }


    // Create patient
    // || Create a new patient using a patient object 
    public async Task<Patient> CreatePatient(Patient patient, ProjectDatabaseConnection? context = null)
    {
        return await _context.ExecuteAsync(async db =>
        {
            // Creates a patient entity and assigns Patient Dto data to that entity
            Patient entity = new()
            {
                FirstName = patient.FirstName,
                MiddleName = patient.MiddleName,
                LastName = patient.LastName,
                DateOfBirth = patient.DateOfBirth,
                PhoneNumber = patient.PhoneNumber,
                Email = patient.Email,
                Address = patient.Address,
                IsDisabled = patient.IsDisabled,
                MedicalOverview = new()
            };

            // Server side validation
            ServerValidatePatient(entity);

            // Add to Datbase, Save Changes, Return new entity
            db.PatientDb.Add(entity);
            await db.SaveChangesAsync();
            return entity;
        }, context);
    }



    // Update Patient
    public async Task<Patient> UpdatePatient(Patient updated, ProjectDatabaseConnection? context = null)
    {
        return await _context.ExecuteAsync(async db =>
        {
            Patient? entity = await GetPatientById(updated.Id, db);
            if (entity == null) throw new InvalidOperationException("No existing patient found to update");

            // Server side validation
            ServerValidatePatient(updated);

            // Add Patient Data
            entity.FirstName = updated.FirstName;
            entity.MiddleName = updated.MiddleName;
            entity.LastName = updated.LastName;
            entity.Email = updated.Email;
            entity.PhoneNumber = updated.PhoneNumber;
            entity.Address = updated.Address;
            entity.DateOfBirth = updated.DateOfBirth;
            entity.IsDisabled = updated.IsDisabled;

            // Add Medications Update
            entity.MedicalOverview!.Medications = updated.MedicalOverview!.Medications;

            // Save changes and return
            await db.SaveChangesAsync();
            return entity;
        }, context);
    }

    // Disable Patient OR Re-enable patient
    public async Task TogglePatientDisable(int id, ProjectDatabaseConnection? context = null)
    {
        await _context.ExecuteAsync(async db =>
        {
            // Search for patient
            Patient? entity = await GetPatientById(id, db);
            if (entity == null) throw new InvalidOperationException("No existing patient found to update");

            // If disabled, then enable. If enabled, then disable
            entity.IsDisabled = !entity.IsDisabled;

            // Save changes and return
            await db.SaveChangesAsync();
        }, context);
    }



    // Validate Patient Data is sound
    // || This method is used to affirm that patient data is safe and valid before adding it to the database.
    private void ServerValidatePatient(Patient patient)
    {
        ValidationContext validationContext = new(patient);
        List<ValidationResult> validationResults = new();

        // Validate patient data
        bool isValid = Validator.TryValidateObject(
            patient,
            validationContext,
            validationResults,
            validateAllProperties: true
        );

        // Return errors if any
        if (!isValid)
        {
            string errors = string.Join("; ", validationResults.Select(r => r.ErrorMessage));
            throw new ValidationException($"Invalid patient: {errors}");
        }
    }

    // Standard patient query
    // || This query string is reusable throughout this page
    private IQueryable<Patient> PatientQuery(ProjectDatabaseConnection context)
    {
        return context.PatientDb
            .Include(x => x.MedicalOverview)
                .ThenInclude(x => x!.Medications);
    }
}