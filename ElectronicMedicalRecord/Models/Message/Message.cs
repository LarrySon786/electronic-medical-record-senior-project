using System.ComponentModel.DataAnnotations;
using ElectronicMedicalRecord.Models;

public class Message
{
    [Key]
    public int Id { get; set; }

    // Properties
    [Required, StringLength(100)]
    public string Content { get; set; } = "";
    public DateTime SendAt { get; set; } = DateTime.UtcNow;

    // Employee Reference
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!; // Sender

    // Chat Reference
    public int ChatId { get; set; }
    public Chat Chat { get; set; } = null!;

}