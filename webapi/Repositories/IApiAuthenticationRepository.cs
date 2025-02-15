using System.Security.Claims;
using WebApi.Models;

namespace WebApi.Repositories
{
    public interface IApiAuthenticationRepository
    {
        Task<Customer?> ValidateCustomerAsync(string email, string password);

        string CreateToken(Customer customer);

        string CreateRefreshToken(Customer customer);

        ClaimsPrincipal? ValidateRefreshToken(string refreshToken);

        Task<dynamic?> GetCustomerAsync(int id);
    }
}