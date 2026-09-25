using System.ComponentModel.DataAnnotations;

public class Medical
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public Patient? Patient { get; set; }
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
    public List<Medication> Medications { get; set; } = new();
}