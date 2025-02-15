using Microsoft.EntityFrameworkCore;
using WebApi.Database;
using WebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly ApplicationDbContext _context;

    public CustomerRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
    {
        return await _context.Customers.AsNoTracking().ToListAsync();
    }

    public async Task<Customer?> GetCustomerByIdAsync(int id)
    {
        var user = await _context.Customers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        return user;
    }

    public async Task<Customer?> AddCustomerAsync(Customer customer)
    {
        try
        {
            _context.Customers.Add(customer);

            await _context.SaveChangesAsync();

            return customer;
        }
        catch (System.Exception)
        {
            throw new Exception("Falha ao cadastrar cliente");
        }
    }

    public async Task UpdateCustomerAsync(Customer customer)
    {
        try
        {
            customer.UpdatedAt = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}";

            _context.Entry(customer).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }
        catch (System.Exception)
        {
            throw new Exception("Falha ao cadastrar cliente");
        }
    }

    public async Task DeleteCustomerAsync(int id)
    {
        try
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer != null)
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
            }
        }
        catch (System.Exception)
        {
            throw new Exception("Falha ao cadastrar cliente");
        }

    }
}