using System.ComponentModel.DataAnnotations;

namespace ElectronicMedicalRecord.Models;

public class Chat
{
    [Key]
    public int Id;

    public List<int> ParticipantIds { get; set; } = new();
    public List<Message> Messages { get; set; } = new();

    public DateTime CreateAt { get; set; } = DateTime.UtcNow;
}