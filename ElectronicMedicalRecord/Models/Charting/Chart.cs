using System.ComponentModel.DataAnnotations;

namespace ElectronicMedicalRecord.Models;

public class Chart
{
    [Key]
    public int Id { get; set; }

    // Patient Reference
    public int PatientId { get; set; }
    public Patient? Patient { get; set; }

    // Practitioner Reference (author of chart)
    public int PractitionerId { get; set; }
    public Employee? Practitioner { get; set; }

    // Properties
    public DateOnly DateCreated { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    public string SubjectiveNotes { get; set; } = string.Empty;
    public string ObjectiveNotes { get; set; } = string.Empty;
    public string Assesment { get; set; } = string.Empty;
    public string Plan { get; set; } = string.Empty;
}