using System.ComponentModel.DataAnnotations;
using ElectronicMedicalRecord.Models;

public enum EmployeeRole
{
    Employee,
    Practitioner,
    Admin
}


public class Employee
{
    [Key]
    public int Id { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public DateOnly DateOfBirth { get; set; }
    public string PhoneNumber { get; set; } = "";
    public bool IsDisabled { get; set; } = false;
    public string ApplicationUserId { get; set; } = null!;
    public ApplicationUser ApplicationUser { get; set; } = null!;
}

