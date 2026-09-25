namespace ElectronicMedicalRecord.Models.Dtos;


public class TestEmployeeUserDto
{
    public string Id { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public bool EmailConfirmed { get; set; }

    public string Role { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public TestEmployeeDetailsDto TestEmployeeModel { get; set; } = new();
}

public class TestEmployeeDetailsDto
{
    public int Id { get; set; }

    public string ApplicationUserId { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;
}