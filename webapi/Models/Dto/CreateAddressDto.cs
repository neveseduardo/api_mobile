using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Dto;

public class CreateAddressDto
{
    [Required(ErrorMessage = "O campo 'Logradouro' é obrigatório!")]
    [StringLength(255, ErrorMessage = "O campo 'Logradouro' deve ter no máximo 255 caracteres.")]
    public string Logradouro { get; set; } = "";

    [Required(ErrorMessage = "O campo 'Cep' é obrigatório!")]
    [RegularExpression(@"^\d{5}-\d{3}$", ErrorMessage = "O campo 'Cep' deve estar no formato 00000-000.")]
    public string Cep { get; set; } = "";

    [Required(ErrorMessage = "O campo 'Cidade' é obrigatório!")]
    [StringLength(100, ErrorMessage = "O campo 'Cidade' deve ter no máximo 100 caracteres.")]
    public string Cidade { get; set; } = "";

    [Required(ErrorMessage = "O campo 'Estado' é obrigatório!")]
    [StringLength(100, ErrorMessage = "O campo 'Estado' deve ter no máximo 100 caracteres.")]
    public string Estado { get; set; } = "";

    [Required(ErrorMessage = "O campo 'Pais' é obrigatório!")]
    [StringLength(100, ErrorMessage = "O campo 'Pais' deve ter no máximo 100 caracteres.")]
    public string Pais { get; set; } = "";

    [Required(ErrorMessage = "O campo 'Numero' é obrigatório!")]
    [StringLength(10, ErrorMessage = "O campo 'Numero' deve ter no máximo 10 caracteres.")]
    public string Numero { get; set; } = "";

    public string? Complemento { get; set; } = "";
}