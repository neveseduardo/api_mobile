namespace WebApi.Models.ViewModels;

public class MedicalCenterViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public string PhoneNumber { get; set; } = "";

    public string? Email { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public AddressViewModel? address { get; set; }
}