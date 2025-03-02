using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models;

[Table("agendamento_avaliacoes")]
public class AppointmentRating
{
    [Key]
    public int Id { get; set; }

    public int Rating { get; set; }

    public string? Comment { get; set; } = "";

    [ForeignKey("Appointment")]
    public int AppointmentId { get; set; }

    public Appointment? Appointment { get; set; }

    [ForeignKey("User")]
    public int UserId { get; set; }

    public User? User { get; set; }

    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
}