using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Repositories;

public interface IWebAuthenticationRepository
{
    Task<User?> ValidateUserAsync(string email, string password);
    Task<bool> SignInAsync(HttpContext httpContext, User user);
    Task SignOutAsync(HttpContext httpContext);
}