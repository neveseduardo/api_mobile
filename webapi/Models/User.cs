using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models;

[Table("usuarios")]
public class User
{
    [Key]
    public int Id { get; init; }

    [Required(ErrorMessage = "Preenchimento do Campo 'nome' Obrigatório!")]
    [Display(Name = "Nome")]
    public string Name { get; set; } = "";

    [Required(ErrorMessage = "Preenchimento do Campo 'email' Obrigatório!")]
    [Display(Name = "E-mail")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "Preenchimento do Campo 'senha' Obrigatório!")]
    [Display(Name = "Senha")]
    public string Password { get; set; } = "";

    public string[] Roles { get; set; } = [];

    public string CreatedAt { get; set; } = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}";

    public string UpdatedAt { get; set; } = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}";
}