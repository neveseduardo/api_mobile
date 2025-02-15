using WebApi.Helpers;
using WebApi.Models;
using WebApi.Database;

namespace WebApi.Database.Seeders;

public class CustomerSeeder
{
    private readonly ApplicationDbContext _context;

    public CustomerSeeder(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Seed()
    {
        if (!_context.Customers.Any(a => a.Email == "email@email.com"))
        {
            var password = PasswordHelper.HashPassword("Senh@123");
            var customer = new Customer { Name = "Cliente", Email = "email@email.com", Password = "" };

            customer.Password = password;

            _context.Customers.Add(customer);
            _context.SaveChanges();
        }
    }
}