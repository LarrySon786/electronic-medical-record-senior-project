using System.ComponentModel.DataAnnotations;

public enum EmployeeRole
{
    Employee,
    Practitioner,
    Admin
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