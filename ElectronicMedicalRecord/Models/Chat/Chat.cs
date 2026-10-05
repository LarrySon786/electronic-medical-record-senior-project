using System.ComponentModel.DataAnnotations;

namespace ElectronicMedicalRecord.Models;

public class Chat
{
    [Key]
    public int Id { get; set; }

    // Reference users in chat
    public List<int> ParticipantIds { get; set; } = new();
    public List<Employee> Participants { get; set; } = new();

    // Reference Messages
    public List<Message> Messages { get; set; } = new();

    // Properties
    public DateTime CreateAt { get; set; } = DateTime.UtcNow;
}