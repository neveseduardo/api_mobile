using Microsoft.AspNetCore.JsonPatch;
using WebApi.Models;

namespace WebApi.Repositories;
public interface IUserRepository
{
    Task<IEnumerable<User>> GetUsersAsync();

    Task<User?> GetUserByIdAsync(int id);

    Task<User?> AddUserAsync(User User);

    Task<User?> DeleteUserAsync(int id);

    Task<User?> UpdateUserAsync(int id, User user);
}