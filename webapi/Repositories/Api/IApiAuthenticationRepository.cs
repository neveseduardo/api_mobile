using System.Security.Claims;
using WebApi.Models;

namespace WebApi.Repositories.Api;
public interface IApiAuthenticationRepository
{
    Task<User?> ValidateUserAsync(string email, string password);

    string CreateToken(User user);

    string CreateRefreshToken(User user);

    ClaimsPrincipal? ValidateRefreshToken(string refreshToken);

    Task<User?> GetUserAsync(int id);

    Task<User> CreateUserAsync(User user);

    Task<bool> FindUserByEmailAsync(string Email);

    Task<Address?> CreateAddressAndBindUser(Address address, int id);
}
