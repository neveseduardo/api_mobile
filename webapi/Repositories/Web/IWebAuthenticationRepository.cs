using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Repositories.Web;

public interface IWebAuthenticationRepository
{
    Task<Administrator?> ValidateAdministratorAsync(string email, string password);
    Task<bool> SignInAsync(HttpContext httpContext, Administrator administrator);
    Task SignOutAsync(HttpContext httpContext);
}