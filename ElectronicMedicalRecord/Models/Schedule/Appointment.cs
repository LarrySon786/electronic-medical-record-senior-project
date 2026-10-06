using System.ComponentModel.DataAnnotations;

namespace ElectronicMedicalRecord.Models;

public class Appointment
{
    [Key]
    public int Id { get; set; }

    // Practitioner Reference
    public required int PractitionerId { get; set; }
    public Employee? Practitioner { get; set; }

    // Patient Refrence
    public int? PatientId { get; set; }
    public Patient? Patient { get; set; }

    // Properties
    [MaxLength(1000, ErrorMessage = "The appointment title cannot exceed 1000 characters.")]
    public string Title { get; set; } = "Appointment";
    public required DateOnly Date { get; set; }
    public required TimeOnly StartTime { get; set; }
    public DateTime DateTime => new DateTime(Date, StartTime);
    public required TimeSpan TimeLength { get; set; }
    public bool Archived { get; set; } = false;
}