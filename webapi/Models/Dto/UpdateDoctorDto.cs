using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Dto;

public class UpdateDoctorDto
{
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres.")]
    public string? Name { get; set; } = "";

    [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$", ErrorMessage = "O campo {0} deve estar no formato 999.999.999-99.")]
    public string? CPF { get; set; } = "";

    [EmailAddress(ErrorMessage = "O campo {0} deve ser um endereço de email válido.")]
    public string? Email { get; set; }

    [RegularExpression(@"^\d{4,6}[A-Za-z]{2}$", ErrorMessage = "O campo {0} deve estar no formato 9999AA ou 999999AA.")]
    public string? CRM { get; set; } = "";

    [Range(1, int.MaxValue, ErrorMessage = "O campo {0} deve ser um valor positivo.")]
    public int? EspecializationId { get; set; }
}