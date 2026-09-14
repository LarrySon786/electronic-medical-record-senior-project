using System.ComponentModel.DataAnnotations;

namespace ElectronicMedicalRecord.Database.Models;

public class TestConnection
{
    [Key]
    public int Id { get; set; }

    public string FirstName { get; set; } = string.Empty;
}