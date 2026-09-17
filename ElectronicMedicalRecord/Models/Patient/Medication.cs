using System.ComponentModel.DataAnnotations;

namespace ElectronicMedicalRecord.Models;

public class Medication
{
    [Key]
    public int Id { get; set; }   

    // Medical Overview Reference
    public int MedicalId { get; set; }
    public Medical? Medical { get; set; }

    // Properties
    [Required]
    public string name { get; set; } = "";
}