using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models;

[Table("clientes")]
public class Customer
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Preenchimento do Campo 'nome' Obrigatório!")]
    public string Name { get; set; } = "";

    [Required(ErrorMessage = "Preenchimento do Campo 'email' Obrigatório!")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "Preenchimento do Campo 'senha' Obrigatório!")]
    public string Password { get; set; } = "";

    [Required(ErrorMessage = "Preenchimento do Campo 'cpf' Obrigatório!")]
    public string Cpf { get; set; } = "";

    public string? CreatedAt { get; set; } = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}";

    public string? UpdatedAt { get; set; } = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}";
}
