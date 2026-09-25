using System.ComponentModel.DataAnnotations;

namespace ElectronicMedicalRecord.Models;

public class Patient
{
    [Key]
    public int Id { get; set; }

    // Reference to Medical Overview
    public int MedicalOverviewId { get; set; }
    public Medical? MedicalOverview { get; set; }


    // Properties
    [Required, StringLength(25)]
    public string FirstName { get; set; } = "";

    [Required, StringLength(25)]
    public string MiddleName { get; set; } = "";

    [Required, StringLength(25)]
    public string LastName { get; set; } = "";

    [Required]
    public string PhoneNumber { get; set; } = "";

    [Required]
    public string Email { get; set; } = "";

    [Required, StringLength(100)]
    public string Address { get; set; } = "";

    [Required(ErrorMessage = "Please enter a date of birth")]
    public DateOnly DateOfBirth { get; set; }

    public bool IsDisabled { get; set; }

}