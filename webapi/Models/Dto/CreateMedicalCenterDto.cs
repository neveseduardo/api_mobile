using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Dto;

public class CreateMedicalCenterDto
{
    [Required(ErrorMessage = "Campo {0} obrigatório.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres.")]
    public string Name { get; set; } = "";

    [Required(ErrorMessage = "Campo {0} obrigatório.")]
    [RegularExpression(@"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$", ErrorMessage = "O campo {0} deve ser um número de telefone válido.")]
    public string PhoneNumber { get; set; } = "";

    [EmailAddress(ErrorMessage = "O campo {0} deve ser um endereço de email válido.")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Campo {0} obrigatório.")]
    [Range(1, int.MaxValue, ErrorMessage = "O campo {0} deve ser um valor positivo.")]
    public int AddressId { get; set; }
}