using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Dto;

public class UpdateAddressDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório!")]
    [MaxLength(255, ErrorMessage = "O campo {0} deve ter no máximo 255 caracteres.")]
    public string Logradouro { get; set; } = "";

    [MinLength(8, ErrorMessage = "O campo {0} deve ter pelo menos {1} caracteres.")]
    public string? Cep { get; set; } = "";

    [MaxLength(255, ErrorMessage = "O campo {0} deve ter no máximo {1} caracteres.")]
    public string? Bairro { get; set; } = "";

    [MaxLength(255, ErrorMessage = "O campo {0} deve ter no máximo {1} caracteres.")]
    public string? Cidade { get; set; } = "";

    [MaxLength(255, ErrorMessage = "O campo {0} deve ter no máximo {1} caracteres.")]
    public string? Estado { get; set; } = "";

    [MaxLength(255, ErrorMessage = "O campo {0} deve ter no máximo {1} caracteres.")]
    public string? Pais { get; set; } = "Brasil";

    [MaxLength(255, ErrorMessage = "O campo {0} deve ter no máximo {1} caracteres.")]
    public string? Numero { get; set; } = "SN";

    [MaxLength(255, ErrorMessage = "O campo {0} deve ter no máximo {1} caracteres.")]
    public string? Complemento { get; set; } = "";
}