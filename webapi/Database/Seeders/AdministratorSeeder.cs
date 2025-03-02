using WebApi.Helpers;
using WebApi.Models;
using WebApi.Database;

namespace WebApi.Database.Seeders;

public class AdministratorSeeder
{
    private readonly ApplicationDbContext _context;

    public AdministratorSeeder(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Seed()
    {
        if (!_context.Administrators.Any(a => a.Email == "administrador@administrador.com"))
        {
            var password = PasswordHelper.HashPassword("Senh@123");
            var Administrator = new Administrator { Name = "Administrador", Email = "administrador@administrador.com", Password = "" };

            Administrator.Password = password;

            _context.Administrators.Add(Administrator);
            _context.SaveChanges();
        }
    }
}