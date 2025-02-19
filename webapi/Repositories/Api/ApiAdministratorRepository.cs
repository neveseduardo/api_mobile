using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;
using WebApi.Models;
using WebApi.Database;

namespace WebApi.Repositories.Api;

public class ApiAdministratorRepository : IApiAdministratorRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ApiAdministratorRepository(ApplicationDbContext dBContext)
    {
        _dbContext = dBContext;
    }

    public async Task<IEnumerable<Administrator>> GetAdministratorsAsync()
    {
        var Administrators = await _dbContext.Administrators.AsNoTracking().ToListAsync();
        return Administrators;
    }

    public async Task<Administrator?> GetAdministratorByIdAsync(int id)
    {
        var Administrator = await _dbContext.Administrators.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return Administrator;
    }

    public async Task<Administrator?> AddAdministratorAsync(Administrator Administrator)
    {
        try
        {
            await _dbContext.Administrators.AddAsync(Administrator);
            await _dbContext.SaveChangesAsync();
            return Administrator;
        }
        catch (System.Exception)
        {
            throw new Exception("Falha ao cadstrar usuario");
        }
    }

    public async Task<Administrator?> DeleteAdministratorAsync(int id)
    {
        var Administrator = await GetAdministratorByIdAsync(id);

        if (Administrator == null)
        {
            return Administrator;
        }

        _dbContext.Administrators.Remove(Administrator);
        await _dbContext.SaveChangesAsync();

        return Administrator;
    }

    public async Task<Administrator?> UpdateAdministratorAsync(int id, Administrator Administrator)
    {
        var AdministratorQuery = await GetAdministratorByIdAsync(id);

        if (AdministratorQuery == null)
        {
            return AdministratorQuery;
        }

        Administrator.UpdatedAt = DateTime.UtcNow;

        _dbContext.Entry(AdministratorQuery).CurrentValues.SetValues(Administrator);
        await _dbContext.SaveChangesAsync();

        return AdministratorQuery;
    }
}