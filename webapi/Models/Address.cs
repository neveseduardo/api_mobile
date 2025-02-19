using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models;

[Table("enderecos")]
public class Address
{
    [Key]
    public int Id { get; set; }

    [Display(Name = "Logradouro")]
    [Required(ErrorMessage = "O campo '{0}' é obrigatório!")]
    [StringLength(255, ErrorMessage = "O campo '{0}' deve ter no máximo {1} caracteres.")]
    public string Logradouro { get; set; } = "";

    [Display(Name = "Cep")]
    [Required(ErrorMessage = "O campo '{0}' é obrigatório!")]
    [RegularExpression(@"^\d{5}-\d{3}$", ErrorMessage = "O campo '{0}' deve estar no formato 00000-000.")]
    public string Cep { get; set; } = "";

    [Display(Name = "Cidade")]
    [Required(ErrorMessage = "O campo '{0}' é obrigatório!")]
    [StringLength(100, ErrorMessage = "O campo '{0}' deve ter no máximo {1} caracteres.")]
    public string Cidade { get; set; } = "";

    [Display(Name = "Estado")]
    [Required(ErrorMessage = "O campo '{0}' é obrigatório!")]
    [StringLength(100, ErrorMessage = "O campo '{0}' deve ter no máximo {1} caracteres.")]
    public string Estado { get; set; } = "";

    [Display(Name = "Pais")]
    [Required(ErrorMessage = "O campo '{0}' é obrigatório!")]
    [StringLength(100, ErrorMessage = "O campo '{0}' deve ter no máximo {1} caracteres.")]
    public string Pais { get; set; } = "";

    [Display(Name = "Numero")]
    [Required(ErrorMessage = "O campo '{0}' é obrigatório!")]
    [StringLength(10, ErrorMessage = "O campo '{0}' deve ter no máximo {1} caracteres.")]
    public string Numero { get; set; } = "";

    [Display(Name = "Complemento")]
    public string? Complemento { get; set; } = "";

    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
}
