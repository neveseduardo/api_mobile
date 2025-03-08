using Microsoft.EntityFrameworkCore;
using WebApi.Database;
using WebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Repositories;

public class UserRepository : IRepository<User>
{
    private readonly ApplicationDbContext _context;

    private readonly ILogger<UserRepository> _logger;

    public UserRepository(ApplicationDbContext context, ILogger<UserRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users
            .OrderByDescending(a => a.Id)
            .ToListAsync();
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<User?> AddAsync(User User)
    {
        try
        {
            _context.Users.Add(User);

            await _context.SaveChangesAsync();

            return User;
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Falha ao adicionar item");
            throw;
        }
    }

    public async Task UpdateAsync(User user)
    {
        try
        {
            user.UpdatedAt = DateTime.UtcNow;
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Falha ao artualizar item");
            throw;
        }
    }

    public async Task DeleteAsync(User user)
    {
        try
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }

    }
}