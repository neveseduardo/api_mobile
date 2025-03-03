using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Dto;

public class CreateMedicalAgreementDto
{
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(100, ErrorMessage = "O nome do provedor deve ter no máximo 100 caracteres.")]
    public string Provider { get; set; } = string.Empty;

    [Required]
    public int HealthPlanId { get; set; }



    [Required]
    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }
}
