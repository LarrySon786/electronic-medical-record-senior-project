public class Medical
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public Patient? Patient { get; set; }
    public List<string> Medications { get; set; } = new List<string>();
}