using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models;

[Table("administradores")]
public class Administrator
{
    [Key]
    public int Id { get; init; }

    [Required(ErrorMessage = "Preenchimento do Campo '{0}' Obrigatório!")]
    [Display(Name = "Nome")]
    [StringLength(100, ErrorMessage = "O campo '{0}' deve ter no máximo {1} caracteres.")]
    public string Name { get; set; } = "";

    [Required(ErrorMessage = "Preenchimento do Campo '{0}' Obrigatório!")]
    [Display(Name = "E-mail")]
    [EmailAddress(ErrorMessage = "O campo '{0}' deve conter um e-mail válido.")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "Preenchimento do Campo '{0}' Obrigatório!")]
    [Display(Name = "Senha")]
    [MinLength(6, ErrorMessage = "O campo '{0}' deve ter pelo menos {1} caracteres.")]
    [MaxLength(50, ErrorMessage = "O campo '{0}' deve ter no máximo {1} caracteres.")]
    public string Password { get; set; } = "";

    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
}
