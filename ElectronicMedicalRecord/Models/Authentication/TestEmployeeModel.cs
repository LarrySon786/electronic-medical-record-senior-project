using System.ComponentModel.DataAnnotations;

namespace ElectronicMedicalRecord.Models;

public class TestEmployeeModel
{
    [Key]
    public int Id { get; set; }

    // Reference application and identity user
    public ApplicationUser? applicationUser { get; set; }
    public string applicationUserId { get; set; } = string.Empty;

    // Properties
    public string FirstName { get; set; } = "Test Model";

    


}