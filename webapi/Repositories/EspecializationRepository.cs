using Microsoft.EntityFrameworkCore;
using WebApi.Database;
using WebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Repositories;

public class EspecializationRepository : IRepository<Especialization>
{
    private readonly ApplicationDbContext _context;

    private readonly ILogger<EspecializationRepository> _logger;

    public EspecializationRepository(ApplicationDbContext context, ILogger<EspecializationRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Especialization>> GetAllAsync()
    {
        return await _context.Especializations
            .OrderByDescending(a => a.Id)
            .ToListAsync();
    }

    public async Task<Especialization?> GetByIdAsync(int id)
    {
        return await _context.Especializations.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Especialization?> AddAsync(Especialization especialization)
    {
        try
        {
            _context.Especializations.Add(especialization);
            await _context.SaveChangesAsync();

            return especialization;
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Falha ao adicionar item");
            throw;
        }
    }

    public async Task UpdateAsync(Especialization especialization)
    {
        try
        {
            especialization.UpdatedAt = DateTime.UtcNow;
            _context.Entry(especialization).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task DeleteAsync(Especialization especialization)
    {
        try
        {
            _context.Especializations.Remove(especialization);
            await _context.SaveChangesAsync();
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}