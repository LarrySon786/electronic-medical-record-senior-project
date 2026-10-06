namespace ElectronicMedicalRecord.Models.Dtos;

public class AppointmentDto
{
    public string Title { get; set; } = string.Empty;
    public Employee? Practitioner { get; set; }
    public Patient? Patient { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeSpan TimeLength { get; set; }
}