using System.Security.Claims;
using WebApi.Models;

namespace WebApi.Repositories.Api;
public interface IApiAuthenticationRepository
{
    Task<User?> ValidateUserAsync(string email, string password);

    string CreateToken(User User);

    string CreateRefreshToken(User User);

    ClaimsPrincipal? ValidateRefreshToken(string refreshToken);

    Task<dynamic?> GetUserAsync(int id);
}
