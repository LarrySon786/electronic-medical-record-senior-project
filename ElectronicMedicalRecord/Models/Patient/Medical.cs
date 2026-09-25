using System.ComponentModel.DataAnnotations;

namespace ElectronicMedicalRecord.Models;

public class Medical
{
    [Key]
    public int Id { get; set; }

    // Patient Reference
    public int PatientId { get; set; }
    public Patient? Patient { get; set; }

    // Medications Reference
    public List<Medication> Medications { get; set; } = new List<Medication>();
}