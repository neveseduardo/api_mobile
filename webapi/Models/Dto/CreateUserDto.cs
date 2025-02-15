using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Dto;
public class CreateUserDto
{
    [Required(ErrorMessage = "Campo obrigatório! {0}")]
    [StringLength(100, ErrorMessage = "Nome não pode ter mais que 100 caracteres")]
    public string Name { get; set; } = "";

    [Required(ErrorMessage = "Campo obrigatório! {0}")]
    [StringLength(100, ErrorMessage = "Nome não pode ter mais que 100 caracteres")]
    [EmailAddress(ErrorMessage = "Formato de email inválido")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "Campo obrigatório! {0}")]
    [StringLength(100, ErrorMessage = "Nome não pode ter mais que 100 caracteres")]
    public string Password { get; set; } = "";
    public string[] Roles { get; set; } = [];
}