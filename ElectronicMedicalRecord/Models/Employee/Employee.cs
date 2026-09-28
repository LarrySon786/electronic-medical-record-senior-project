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

public class Employee
{
    public int Id { get; set; }
    [Required, StringLength(25)] public string FirstName { get; set; } = "";
    [StringLength(25)] public string MiddleName { get; set; } = "";
    [Required, StringLength(25)] public string LastName { get; set; } = "";
    [Required] public string PhoneNumber { get; set; } = "";
    [Required, EmailAddress] public string Email { get; set; } = "";
    [Required, StringLength(100)] public string Address { get; set; } = "";
    [DataType(DataType.Date)] public DateTime DateOfBirth { get; set; } = DateTime.Today.AddYears(-30);
    [Required, MinLength(8)] public string InitialPassword { get; set; } = "";
    public EmployeeRole Role { get; set; } = EmployeeRole.Employee;
    public bool IsDisabled { get; set; }
}
