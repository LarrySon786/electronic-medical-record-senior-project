using System.ComponentModel.DataAnnotations;
using ElectronicMedicalRecord.Models;
using ElectronicMedicalRecord.Models.Dtos;


public class Employee
{
    [Key]
    public int Id { get; set; }

    public string ApplicationUserId { get; set; } = null!;
    public ApplicationUser ApplicationUser { get; set; } = null!;

    [Required, StringLength(25)] public string FirstName { get; set; } = "";

    [StringLength(25)] public string MiddleName { get; set; } = "";

    [Required, StringLength(25)] public string LastName { get; set; } = "";

    [Required] public string PhoneNumber { get; set; } = "";

    [DataType(DataType.Date)] public DateOnly DateOfBirth { get; set; } = DateOnly.FromDateTime(DateTime.Today.AddYears(-30));

    public EmployeeRole Role { get; set; } = EmployeeRole.Employee;

    public bool IsDisabled { get; set; }
}
