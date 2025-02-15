namespace WebApi.Models.ViewModels;
public class UserViewModel
{
    public int Id { get; init; }
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string[] Roles { get; set; } = [];
}

