using Microsoft.EntityFrameworkCore;
using WebApi.Database;
using WebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Repositories;

public class MedicalAgreementRepository : IRepository<MedicalAgreement>
{
    private readonly ApplicationDbContext _context;

    private readonly ILogger<MedicalAgreementRepository> _logger;

    public MedicalAgreementRepository(ApplicationDbContext context, ILogger<MedicalAgreementRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<MedicalAgreement>> GetAllAsync()
    {
        return await _context.MedicalAgreements
            .OrderByDescending(a => a.Id)
            .Include(r => r.HealthPlan)
            .ToListAsync();
    }

    public async Task<MedicalAgreement?> GetByIdAsync(int id)
    {
        return await _context.MedicalAgreements
            .Include(r => r.HealthPlan)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<MedicalAgreement?> AddAsync(MedicalAgreement medicalagreement)
    {
        try
        {
            _context.MedicalAgreements.Add(medicalagreement);
            await _context.SaveChangesAsync();

            return medicalagreement;
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Falha ao adicionar item");
            throw;
        }
    }

    public async Task UpdateAsync(MedicalAgreement medicalagreement)
    {
        try
        {
            medicalagreement.UpdatedAt = DateTime.UtcNow;
            _context.Entry(medicalagreement).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Falha ao atualizar item");
            throw;
        }
    }

    public async Task DeleteAsync(MedicalAgreement medicalagreement)
    {
        try
        {
            _context.MedicalAgreements.Remove(medicalagreement);
            await _context.SaveChangesAsync();
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Falha ao deletar item");
            throw;
        }
    }
}