using System.ComponentModel.DataAnnotations;
using ElectronicMedicalRecord.Database;
using ElectronicMedicalRecord.DTOs;
using ElectronicMedicalRecord.Models;
using ElectronicMedicalRecord.Models.Dtos;
using ElectronicMedicalRecord.Services.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ElectronicMedicalRecord.Services;

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
    public async Task<Employee?> GetEmployeeByIdAsync(int id, ProjectDatabaseConnection? context = null)
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

    // Create New Employee w/o DTO
    public async Task<Employee> CreateEmployeeAsync(Employee employee, string Password, ProjectDatabaseConnection? context = null)
    {
        return await _context.ExecuteAsync(async db =>
        {
            // Server side validation
            ServerValidateEmployee(employee);

            if (employee.ApplicationUser.Email == null) throw new Exception("Could not create employee. Email not existing");

            var existingUser = await _userManager.FindByEmailAsync(employee.ApplicationUser.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("Account already exist");
            }

            //Create New Employee account
            ApplicationUser newUser = new()
            {
                UserName = employee.ApplicationUser.Email,
                Email = employee.ApplicationUser.Email,
                EmailConfirmed = true,
            };

            var result = await _userManager.CreateAsync(newUser, Password);

            if (!result.Succeeded)
            {
                string errors = string.Join(",", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException(errors);
            }

            // add role to new Employee account
            await _userManager.AddToRoleAsync(newUser, employee.Role.ToString());

            Employee entity = new()
            {
                FirstName = employee.FirstName,
                MiddleName = employee.MiddleName,
                LastName = employee.LastName,
                DateOfBirth = employee.DateOfBirth,
                PhoneNumber = employee.PhoneNumber,
                Role = employee.Role,
                ApplicationUserId = newUser.Id
            };

            // Add new Employee to Datbase, Save Changes, Return new entity
            db.EmployeeDb.Add(entity);
            await db.SaveChangesAsync();

            return entity;

        }, context);
    }

    // Create New Employee
    // || Create a new Employee using a CreateEmployeeDto object
    public async Task<Employee> CreateEmployeeByDtoAsync(CreateEmployeeDto employeeDto, ProjectDatabaseConnection? context = null)
    {
        return await _context.ExecuteAsync(async db =>
        {
            // Server side validation
            ServerValidateEmployee(employeeDto);

            var existingUser = await _userManager.FindByEmailAsync(employeeDto.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("Account already exist");
            }

            //Create New Employee account
            ApplicationUser newUser = new()
            {
                UserName = employeeDto.Email,
                Email = employeeDto.Email,
                EmailConfirmed = true,
            };

            var result = await _userManager.CreateAsync(newUser, employeeDto.Password);

            if (!result.Succeeded)
            {
                string errors = string.Join(",", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException(errors);
            }

            // add role to new Employee account
            await _userManager.AddToRoleAsync(newUser, employeeDto.Role.ToString());

            Employee entity = new()
            {
                FirstName = employeeDto.FirstName,
                MiddleName = employeeDto.MiddleName,
                LastName = employeeDto.LastName,
                DateOfBirth = employeeDto.DateOfBirth,
                PhoneNumber = employeeDto.PhoneNumber,
                Role = employeeDto.Role,
                ApplicationUserId = newUser.Id
            };

            // Add new Employee to Datbase, Save Changes, Return new entity
            db.EmployeeDb.Add(entity);
            await db.SaveChangesAsync();

            return entity;

        }, context);
    }

    // Update Employee
    public async Task<Employee> UpdateEmployeeAsync(Employee employee, ProjectDatabaseConnection? context = null)
    {
        return await _context.ExecuteAsync(async db =>
        {
            if (employee.ApplicationUser.Email == null) throw new Exception("Employee must have an included email.");

            Employee? existingEmployee = await GetEmployeeByIdAsync(employee.Id, db);
            if (existingEmployee == null) throw new InvalidOperationException("No existing employee found to update");

            // Server side validation
            ServerValidateEmployee(employee);

            // Get the current user account
            var user = await _userManager.FindByIdAsync(existingEmployee.ApplicationUserId);
            if (user == null) throw new InvalidOperationException("No existing employee account found to update");

            //Check existing email
            if (!employee.ApplicationUser.Email.Equals(user.Email, StringComparison.CurrentCultureIgnoreCase))
            {
                var existingUser = await _userManager.FindByEmailAsync(employee.ApplicationUser.Email);

                // Email belongs to another user
                if (existingUser != null &&
                    existingUser.Id != user.Id)
                {
                    throw new InvalidOperationException(
                        "This email is already used by another user");
                }

                // Update Identity account
                user.Email = employee.ApplicationUser.Email;
                user.UserName = employee.ApplicationUser.Email;

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
            var currentRoles = await _userManager.GetRolesAsync(user);

            string newRole = employee.Role.ToString();

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
            existingEmployee.FirstName = employee.FirstName;
            existingEmployee.MiddleName = employee.MiddleName;
            existingEmployee.LastName = employee.LastName;
            existingEmployee.DateOfBirth = employee.DateOfBirth;
            existingEmployee.PhoneNumber = employee.PhoneNumber;
            existingEmployee.IsDisabled = employee.IsDisabled;
            existingEmployee.Role = employee.Role;

            // Save changes and return
            await db.SaveChangesAsync();
            return existingEmployee;

        }, context);
    }

    public async Task<Employee> UpdateEmployeeByDtoAsync(UpdateEmployeeDto employeeDto, ProjectDatabaseConnection? context = null)
    {
        return await _context.ExecuteAsync(async db =>
        {
            Employee? existingEmployee = await GetEmployeeByIdAsync(employeeDto.Id, db);
            if (existingEmployee == null) throw new InvalidOperationException("No existing employee found to update");

            // Server side validation
            ServerValidateEmployee(employeeDto);

            // Get the current user account
            var user = await _userManager.FindByIdAsync(existingEmployee.ApplicationUserId);
            if (user == null) throw new InvalidOperationException("No existing employee account found to update");

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
            var currentRoles = await _userManager.GetRolesAsync(user);

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
            existingEmployee.MiddleName = employeeDto.MiddleName;
            existingEmployee.LastName = employeeDto.LastName;
            existingEmployee.DateOfBirth = employeeDto.DateOfBirth;
            existingEmployee.PhoneNumber = employeeDto.PhoneNumber;
            existingEmployee.IsDisabled = employeeDto.IsDisabled;
            existingEmployee.Role = employeeDto.Role;

            // Save changes and return
            await db.SaveChangesAsync();
            return existingEmployee;

        }, context);
    }

    // Disable Employee OR Re-enable Employee
    public async Task ToggleEmployeeDisable(int id, ProjectDatabaseConnection? context = null)
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


    // Validate Employee Data is sound
    // || This method is used to affirm that employee data is safe and valid before adding it to the database.
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