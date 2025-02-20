using Microsoft.EntityFrameworkCore;
using WebApi.Database;
using WebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Repositories.Api;

public class ApiUserRepository : IApiUserRepository
{
    private readonly ApplicationDbContext _context;
    public ApiUserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        var users = await _context.Users
            .AsNoTracking()
            .Include(u => u.address)
            .ToListAsync();

        return users;
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        var user = await _context.Users
            .AsNoTracking()
            .Include(u => u.address)
            .FirstOrDefaultAsync(x => x.Id == id);

        return user;
    }

    public async Task<User?> AddUserAsync(User User)
    {
        try
        {
            _context.Users.Add(User);

            await _context.SaveChangesAsync();

            return User;
        }
        catch (System.Exception)
        {
            throw new Exception("Falha ao cadastrar cliente");
        }
    }

    public async Task UpdateUserAsync(User user)
    {
        try
        {
            user.UpdatedAt = DateTime.UtcNow;

            _context.Entry(user).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }
        catch (System.Exception)
        {
            throw new Exception("Falha ao cadastrar cliente");
        }
    }

    public async Task DeleteUserAsync(int id)
    {
        try
        {
            var User = await _context.Users.FindAsync(id);

            if (User != null)
            {
                _context.Users.Remove(User);
                await _context.SaveChangesAsync();
            }
        }
        catch (System.Exception)
        {
            throw new Exception("Falha ao cadastrar cliente");
        }

    }
}