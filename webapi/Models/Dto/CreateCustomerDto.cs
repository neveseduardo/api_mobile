using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Dto;

public class CreateCustomerDto
{
    [Required(ErrorMessage = "Preenchimento do Campo 'nome' Obrigatório!")]
    public string Name { get; set; } = "";

    [Required(ErrorMessage = "Preenchimento do Campo 'email' Obrigatório!")]
    [EmailAddress(ErrorMessage = "O email não é válido.")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "Campo obrigatório! {0}")]
    [StringLength(100, ErrorMessage = "Nome não pode ter mais que 100 caracteres")]
    public string Password { get; set; } = "";

    [Required(ErrorMessage = "Preenchimento do Campo 'cpf' Obrigatório!")]
    public string Cpf { get; set; } = "";
}
