using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models;

[Table("enderecos_usuarios")]
public class UserAddress
{
    [Key]
    public int Id { get; set; }

    public int? PrincipalId { get; set; }

    [ForeignKey("address")]
    public Nullable<int> AddressId { get; set; }

    public Address? address { get; set; }

    [ForeignKey("user")]
    public Nullable<int> UserId { get; set; }

    public User? user { get; set; }

    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
}
