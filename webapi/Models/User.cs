using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models;

[Table("usuarios")]
public class User
{
    [Key]
    public int Id { get; set; }

    [Display(Name = "Nome")]
    [Required(ErrorMessage = "Preenchimento do Campo '{0}' Obrigatório!")]
    public string Name { get; set; } = null!;

    [Display(Name = "Email")]
    [Required(ErrorMessage = "Preenchimento do Campo '{0}' Obrigatório!")]
    public string Email { get; set; } = null!;

    [Display(Name = "Senha")]
    [Required(ErrorMessage = "Preenchimento do Campo '{0}' Obrigatório!")]
    public string Password { get; set; } = null!;

    [Display(Name = "CPF")]
    [Required(ErrorMessage = "Preenchimento do Campo '{0}' Obrigatório!")]
    public string Cpf { get; set; } = null!;


    [Display(Name = "AddressId")]
    [ForeignKey("address")]
    public Nullable<int> AddressId { get; set; }

    public Address? address { get; set; }

    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
}
