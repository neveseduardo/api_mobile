using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Dto;

public class UpdateAddressDto
{
    public string? Logradouro { get; set; } = "";

    public string? Cep { get; set; } = "";

    public string? Cidade { get; set; } = "";

    public string? Estado { get; set; } = "";

    public string? Pais { get; set; } = "";

    public string? Numero { get; set; } = "";

    public string? Complemento { get; set; } = "";
}