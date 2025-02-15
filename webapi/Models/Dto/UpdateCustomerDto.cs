using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Dto;

public class UpdateCustomerDto
{
    [Required(ErrorMessage = "Preenchimento do Campo 'nome' Obrigatório!")]
    public string Name { get; set; } = "";

    [Required(ErrorMessage = "Preenchimento do Campo 'email' Obrigatório!")]
    [EmailAddress(ErrorMessage = "O email não é válido.")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "Preenchimento do Campo 'cpf' Obrigatório!")]
    public string Cpf { get; set; } = "";
}
