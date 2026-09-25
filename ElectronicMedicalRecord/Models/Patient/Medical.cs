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

    // Properties
    [StringLength(10)]
    public string BloodType { get; set; } = "";
    [StringLength(20)]
    public string Height { get; set; } = "";
    [StringLength(20)]
    public string Weight { get; set; } = "";
    [StringLength(500)]
    public string PrimaryHealthcareConcern { get; set; } = "";
    [StringLength(500)]
    public string Allergies { get; set; } = "";
}