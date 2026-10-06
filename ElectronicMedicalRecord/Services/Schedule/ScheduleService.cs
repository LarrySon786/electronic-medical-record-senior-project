using System.ComponentModel.DataAnnotations;
using System.Data;
using ElectronicMedicalRecord.Components;
using ElectronicMedicalRecord.Database;
using ElectronicMedicalRecord.Models;
using ElectronicMedicalRecord.Models.Dtos;
using ElectronicMedicalRecord.Services.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ElectronicMedicalRecord.Services;

public class ScheduleService
{
    private readonly DbContextFactoryHelper _factory;

    public ScheduleService(DbContextFactoryHelper factory)
    {
        _factory = factory;
    }

    
    // GET all appointments within date range
    public async Task<List<Appointment>> GetAllAppointmentsWithinDates(DateOnly StartDate, int NumberOfDays, ProjectDatabaseConnection? context = null)
    {
        return await _factory.ExecuteAsync(async db =>
        {
            return await AppointmentQuery(db)
                .Where(x => x.Date >= StartDate && x.Date <= x.Date.AddDays(NumberOfDays))
                .ToListAsync();
        }, context);
    }

    
    // GET appointment by Id
    public async Task<Appointment?> GetAppointmentById(int id, ProjectDatabaseConnection? context = null)
    {
        return await _factory.ExecuteAsync(async db =>
        {
            return await AppointmentQuery(db)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }, context);
    }

    
    // CREATE new appointment
    public async Task<Appointment?> CreateAppointment(AppointmentDto appointment, ProjectDatabaseConnection? context = null)
    {
        return await _factory.ExecuteAsync(async db =>
        {
            if (appointment.Practitioner == null)
                throw new ValidationException("Appointment must have a practitioner.");

            // Create appointment entity
            var entity = new Appointment()
            {
                PractitionerId = appointment.Practitioner.Id,
                PatientId = appointment.Patient?.Id,
                Title = appointment.Title,
                Date = appointment.Date,
                StartTime = appointment.StartTime,
                TimeLength = appointment.TimeLength,
            };

            // Add default title
            if (appointment.Patient != null)
                entity.Title += $" | {appointment.Patient.FirstName} {appointment.Patient.MiddleName} {appointment.Patient.LastName} | {appointment.Patient.DateOfBirth}";


            // Validate Appointment
            var errors = ValidateAppointment(entity);
            if (errors.Count() > 0)
            {
                var errorMessage = string.Join(
                    Environment.NewLine,
                    errors.Select(err => err.ErrorMessage)
                );

                throw new ValidationException($"Could not create appointment. {errorMessage}");
            }

            // Save and return
            db.Add(entity);
            await db.SaveChangesAsync();
            return entity;
        }, context);
    }

    
    // Update existing appointment
    public async Task<Appointment?> UpdateAppointment(Appointment updated, ProjectDatabaseConnection? context = null)
    {
        return await _factory.ExecuteAsync(async db =>
        {
            // Check if existing
            var existing = await GetAppointmentById(updated.Id, db);
            if (existing == null) throw new Exception("No existing appointment found to update.");

            // Update entity
            existing.Title = updated.Title;
            existing.TimeLength = updated.TimeLength;
            existing.Date = updated.Date;
            existing.StartTime = updated.StartTime;
            existing.PatientId = updated.PatientId;
            existing.PractitionerId = updated.PractitionerId;
            existing.Archived = updated.Archived;

            // Validate Appointment
            var errors = ValidateAppointment(existing);
            if (errors.Count() > 0)
            {
                var errorMessage = string.Join(
                    Environment.NewLine,
                    errors.Select(err => err.ErrorMessage)
                );

                throw new ValidationException($"Could not create appointment. {errorMessage}");
            }

            // Save and return
            await db.SaveChangesAsync();
            
            // Reload appointment with updated navigation properties
            return await AppointmentQuery(db)
                .FirstAsync(x => x.Id == existing.Id);
        }, context);
    }
    
    
    // Archive existing appointment
    public async Task<Appointment?> ToggleAppointmentArchived(int id, ProjectDatabaseConnection? context = null)
    {
        return await _factory.ExecuteAsync(async db =>
        {
            // Check if existing
            var existing = await GetAppointmentById(id, db);
            if (existing == null) throw new Exception("No existing appointment found to update.");

            // Update entity
            existing.Archived = !existing.Archived;

            // Save and return
            await db.SaveChangesAsync();
            return existing;
        }, context);
    }

    // Standard appointment query
    private IQueryable<Appointment> AppointmentQuery(ProjectDatabaseConnection context)
    {
        return context.AppointmentDb
            .Include(x => x.Practitioner)
            .Include(x => x.Patient);
    }

    // Server side validation
    private List<ValidationResult> ValidateAppointment(Appointment appointment)
    {
        var errors = new List<ValidationResult>();

        var context = new ValidationContext(appointment);
        Validator.TryValidateObject(appointment, context, errors, validateAllProperties: true);

        return errors;
    }

}