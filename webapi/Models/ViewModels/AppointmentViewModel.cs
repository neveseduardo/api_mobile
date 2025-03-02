namespace WebApi.Models.ViewModels;

public class AppointmentViewModel
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public string? Notes { get; set; }

    public string Status { get; set; } = "";

    public int UserId { get; set; }

    public int DoctorId { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public DoctorViewModel? Doctor { get; set; }
    public UserViewModel? User { get; set; }
    public AppointmentRatingViewModel? AppointmentRating { get; set; }
}