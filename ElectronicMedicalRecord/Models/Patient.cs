using System.ComponentModel.DataAnnotations;

public class Patient
{
    public int Id { get; set; }

    [Required, StringLength(25)]
    public string FirstName { get; set; } = "";

    [Required, StringLength(25)]
    public string MiddleName { get; set; } = "";

    [Required, StringLength(25)]
    public string LastName { get; set; } = "";

    [Required]
    public string PhoneNumber { get; set; } = "";

    [Required]
    public string Email { get; set; } = "";

    [Required, StringLength(100)]
    public string Address { get; set; } = "";

    public DateTime DateOfBirth { get; set; }

    public bool IsDisabled { get; set; }

}