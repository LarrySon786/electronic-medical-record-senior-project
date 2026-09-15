using System.ComponentModel.DataAnnotations;

namespace ElectronicMedicalRecord.Models.Dtos;

public class PatientDto
{
    [Key]
    public int Id { get; set; }

    // Reference to Medical Overview
    public int MedicalOverviewId { get; set; }
    public Medical? MedicalOverview { get; set; }


    // Properties
    public string FirstName { get; set; } = "";

    public string MiddleName { get; set; } = "";

    public string LastName { get; set; } = "";

    public string PhoneNumber { get; set; } = "";

    public string Email { get; set; } = "";

    public string Address { get; set; } = "";

    public DateOnly DateOfBirth { get; set; }

    public bool IsDisabled { get; set; }

}