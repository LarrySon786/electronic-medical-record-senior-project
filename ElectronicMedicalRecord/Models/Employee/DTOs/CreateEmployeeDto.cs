using System.ComponentModel.DataAnnotations;

namespace ElectronicMedicalRecord.Models.Dtos;

public class CreateEmployeeDto
{
    [Required, StringLength(25)]
    public string FirstName { get; set; } = "";

    [Required, StringLength(25)]
    public string LastName { get; set; } = "";

    [Required, StringLength(8)]
    public string Password { get; set; } = "";

    [Required]
    public DateOnly DateOfBirth { get; set; }

    [Phone, Required]
    public string PhoneNumber { get; set; } = "";

    [Required, EmailAddress]
    public string Email { get; set; } = "";

    public EmployeeRole Role { get; set; } = EmployeeRole.Employee;
}
