using System.ComponentModel.DataAnnotations;
using ElectronicMedicalRecord.Database;
using ElectronicMedicalRecord.DTOs;
using ElectronicMedicalRecord.Models;
using ElectronicMedicalRecord.Models.Dtos;
using ElectronicMedicalRecord.Services.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class EmployeeService
{
    private readonly DbContextFactoryHelper _context;

    private readonly UserManager<ApplicationUser> _userManager;

    public EmployeeService(DbContextFactoryHelper context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    //GET All Employee
    public async Task<List<Employee>> GetAllEmployeesAsync(ProjectDatabaseConnection? context = null)
    {
        return await _context.ExecuteAsync(async db =>
        {
            return await EmployeeQuery(db).ToListAsync();
        }, context);
    }

    //GET Employee By ID
    public async Task<Employee?> GetEmployeeByIdAsync(int id, ProjectDatabaseConnection context = null!)
    {
        return await _context.ExecuteAsync(async db =>
        {
            return await EmployeeQuery(db)
                .Where(e => e.Id == id)
                .FirstOrDefaultAsync();
        }, context);
    }

    //GET Employee by Email
    public async Task<Employee?> GetEmployeeByEmailAsync(string email, ProjectDatabaseConnection? context = null)
    {
        return await _context.ExecuteAsync(async db =>
        {
            return await EmployeeQuery(db)
                .Where(e => e.ApplicationUser.Email == email)
                .FirstOrDefaultAsync();
        }, context);
    }

    // Create New Employee
    // || Create a new Employee using a CreateEmployeeDto object
    public async Task<Employee> CreateEmployeeAsync(CreateEmployeeDto employeeDto, ProjectDatabaseConnection? context = null)
    {
        return await _context.ExecuteAsync(async db =>
        {
            // Server side validation
            ServerValidateEmployee(employeeDto);

            var existingUser = _userManager.FindByEmailAsync(employeeDto.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("Account already exist");
            }

            //Create New Employee account
            ApplicationUser newUser = new()
            {
                UserName = employeeDto.Email,
                Email = employeeDto.Email
            };

            var result = await _userManager.CreateAsync(newUser, employeeDto.Password);

            if (!result.Succeeded)
            {
                string errors = string.Join(",", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException(errors);
            }

            //add role to new Employee account
            await _userManager.AddToRoleAsync(
                newUser,
                employeeDto.Role.ToString()
            );

            Employee entity = new()
            {
                FirstName = employeeDto.FirstName,
                LastName = employeeDto.LastName,
                DateOfBirth = employeeDto.DateOfBirth,
                PhoneNumber = employeeDto.PhoneNumber,
                ApplicationUserId = newUser.Id
            };

            // Add new Employee to Datbase, Save Changes, Return new entity
            db.EmployeeDb.Add(entity);
            await db.SaveChangesAsync();

            return entity;

        }, context);
    }

    // Update Employee
    public async Task<Employee> UpdateEmployeeAsync(UpdateEmployeeDto employeeDto, ProjectDatabaseConnection? context = null)
    {
        return await _context.ExecuteAsync(async db =>
        {
            Employee? existingEmployee = await GetEmployeeByIdAsync(employeeDto.Id, db);
            if (existingEmployee == null) throw new InvalidOperationException("No existing employee found to update");

            // Server side validation
            ServerValidateEmployee(employeeDto);

            // Get the current user account
            var user = await _userManager.FindByIdAsync(existingEmployee.ApplicationUserId);
            if (user == null) throw new InvalidOperationException("No existing employee aacount found to update");

            //Check existing email
            if (!employeeDto.Email.Equals(user.Email, StringComparison.CurrentCultureIgnoreCase))
            {
                var existingUser = await _userManager.FindByEmailAsync(employeeDto.Email);

                // Email belongs to another user
                if (existingUser != null &&
                    existingUser.Id != user.Id)
                {
                    throw new InvalidOperationException(
                        "This email is already used by another user");
                }

                // Update Identity account
                user.Email = employeeDto.Email;
                user.UserName = employeeDto.Email;

                var updateResult =
                    await _userManager.UpdateAsync(user);

                if (!updateResult.Succeeded)
                {
                    string errors = string.Join(
                        ", ",
                        updateResult.Errors.Select(e => e.Description));

                    throw new InvalidOperationException(errors);
                }
            }


            // 5. Get current Identity role
            var currentRoles =
                await _userManager.GetRolesAsync(user);

            string newRole = employeeDto.Role.ToString();

            // Since an employee can have only ONE role,
            // remove the current role and add the new one
            if (!currentRoles.Contains(newRole))
            {
                if (currentRoles.Count > 0)
                {
                    var removeResult =
                        await _userManager.RemoveFromRolesAsync(
                            user,
                            currentRoles);

                    if (!removeResult.Succeeded)
                    {
                        string errors = string.Join(
                            ", ",
                            removeResult.Errors.Select(e => e.Description));

                        throw new InvalidOperationException(errors);
                    }
                }

                var addResult =
                    await _userManager.AddToRoleAsync(
                        user,
                        newRole);

                if (!addResult.Succeeded)
                {
                    string errors = string.Join(
                        ", ",
                        addResult.Errors.Select(e => e.Description));

                    throw new InvalidOperationException(errors);
                }
            }

            // 6. Update Employee
            existingEmployee.FirstName = employeeDto.FirstName;
            existingEmployee.LastName = employeeDto.LastName;
            existingEmployee.DateOfBirth = employeeDto.DateOfBirth;
            existingEmployee.PhoneNumber = employeeDto.PhoneNumber;
            existingEmployee.IsDisabled = employeeDto.IsDisabled;

            // Save changes and return
            await db.SaveChangesAsync();
            return existingEmployee;

        }, context);
    }

    // Disable Patient OR Re-enable patient
    public async Task TogglePatientDisable(int id, ProjectDatabaseConnection? context = null)
    {
        await _context.ExecuteAsync(async db =>
        {
            // Search for Employee
            Employee? entity = await GetEmployeeByIdAsync(id, db);
            if (entity == null) throw new InvalidOperationException("No existing employee found to update");

            // If disabled, then enable. If enabled, then disable
            entity.IsDisabled = !entity.IsDisabled;

            // Save changes and return
            await db.SaveChangesAsync();
        }, context);
    }


    // Validate Patient Data is sound
    // || This method is used to affirm that patient data is safe and valid before adding it to the database.
    private void ServerValidateEmployee<T>(T employee)
    {
        ValidationContext validationContext = new(employee!);
        List<ValidationResult> validationResults = new();

        bool isValid = Validator.TryValidateObject(
            employee!,
            validationContext,
            validationResults,
            validateAllProperties: true
        );

        // Return errors if any
        if (!isValid)
        {
            string errors = string.Join(",", validationResults.Select(r => r.ErrorMessage));
            throw new ValidationException($"Invalid Employee: {errors}");
        }
    }

    // Standard Employee query
    // || This query string is reusable throughout this page
    private IQueryable<Employee> EmployeeQuery(ProjectDatabaseConnection context)
    {
        return context.EmployeeDb.Include(a => a.ApplicationUser);
    }


}