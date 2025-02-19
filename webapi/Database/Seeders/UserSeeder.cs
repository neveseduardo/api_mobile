using WebApi.Helpers;
using WebApi.Models;
using WebApi.Database;

namespace WebApi.Database.Seeders;

public class UserSeeder
{
    private readonly ApplicationDbContext _context;

    public UserSeeder(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Seed()
    {
        if (!_context.Users.Any(a => a.Email == "email@email.com"))
        {
            var password = PasswordHelper.HashPassword("Senh@123");
            var User = new User { Name = "Cliente", Email = "email@email.com", Password = "", Cpf = "701.734.621-11" };

            User.Password = password;

            _context.Users.Add(User);
            _context.SaveChanges();
        }
    }
}