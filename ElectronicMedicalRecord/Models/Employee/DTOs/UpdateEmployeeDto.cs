using System.ComponentModel.DataAnnotations;
using ElectronicMedicalRecord.Models.Dtos;

namespace ElectronicMedicalRecord.DTOs;

public class UpdateEmployeeDto
{
    public int Id { get; }

    [Required]
    public string FirstName { get; set; } = "";

    [Required, StringLength(25)]
    public string MiddleName { get; set; } = "";

    [Required]
    public string LastName { get; set; } = "";

    [Required]
    public DateOnly DateOfBirth { get; set; }

    [Phone]
    public string PhoneNumber { get; set; } = "";

    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";

    public bool IsDisabled { get; set; }

    [EnumDataType(typeof(EmployeeRole))]
    public EmployeeRole Role { get; set; }
}
