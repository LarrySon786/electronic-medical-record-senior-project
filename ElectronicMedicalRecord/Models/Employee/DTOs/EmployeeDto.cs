namespace ElectronicMedicalRecord.Models.Dtos;

public class EmployeeDto
{
    public int Id { get; set; }

    public string FirstName { get; set; } = "";

    public string LastName { get; set; } = "";

    public DateOnly DateOfBirth { get; set; }

    public string PhoneNumber { get; set; } = "";

    public string Email { get; set; } = "";

    public bool Disable { get; set; }

    public string ApplicationUserId { get; set; } = "";
}
