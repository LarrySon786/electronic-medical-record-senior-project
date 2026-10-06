using System.ComponentModel.DataAnnotations;

namespace ElectronicMedicalRecord.Models;

public class SchedulingBlock
{
    [Key]
    public int Id { get; set; }

    // Practitioner
    public int PractitionerId { get; set; }
    public Employee? Practitioner { get; set; }

    // Properties
    public DateTime Time { get; set; }

    public TimeSpan BlockSize { get; set; } = TimeSpan.FromMinutes(15);

}