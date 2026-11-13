namespace MedicalApi.Models;

public class Patient
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public DateOnly BirthDate { get; set; }
    public string? Phone { get; set; }
    public string Address { get; set; } = "";
    public bool HasAllergies { get; set; }
    public string? Email { get; set; }
}





