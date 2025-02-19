using Microsoft.AspNetCore.JsonPatch;
using WebApi.Models;

namespace WebApi.Repositories.Api;
public interface IApiAdministratorRepository
{
    Task<IEnumerable<Administrator>> GetAdministratorsAsync();

    Task<Administrator?> GetAdministratorByIdAsync(int id);

    Task<Administrator?> AddAdministratorAsync(Administrator administrator);

    Task<Administrator?> DeleteAdministratorAsync(int id);

    Task<Administrator?> UpdateAdministratorAsync(int id, Administrator administrator);
}