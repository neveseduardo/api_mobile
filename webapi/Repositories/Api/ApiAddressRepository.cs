using Microsoft.EntityFrameworkCore;
using WebApi.Database;
using WebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Repositories.Api;

public class ApiAddressRepository : IApiAddressRepository
{
    private readonly ApplicationDbContext _context;

    public ApiAddressRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Address>> GetAllAddressesAsync()
    {
        return await _context.Addresses.AsNoTracking().ToListAsync();
    }

    public async Task<Address?> GetAddressByIdAsync(int id)
    {
        var address = await _context.Addresses.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        return address;
    }

    public async Task<Address?> AddAddressAsync(Address address)
    {
        try
        {
            address.CreatedAt = DateTime.UtcNow;
            address.UpdatedAt = DateTime.UtcNow;

            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();

            return address;
        }
        catch (System.Exception)
        {
            throw new Exception("Falha ao cadastrar endereço.");
        }
    }

    public async Task UpdateAddressAsync(Address address)
    {
        try
        {
            address.UpdatedAt = DateTime.UtcNow;

            _context.Entry(address).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        catch (System.Exception)
        {
            throw new Exception("Falha ao atualizar endereço.");
        }
    }

    public async Task DeleteAddressAsync(int id)
    {
        try
        {
            var address = await _context.Addresses.FindAsync(id);

            if (address != null)
            {
                _context.Addresses.Remove(address);
                await _context.SaveChangesAsync();
            }
        }
        catch (System.Exception)
        {
            throw new Exception("Falha ao excluir endereço.");
        }
    }
}