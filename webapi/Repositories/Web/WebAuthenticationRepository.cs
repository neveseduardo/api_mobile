using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApi.Database;
using WebApi.Helpers;
using WebApi.Models;

namespace WebApi.Repositories.Web;

public class WebAuthenticationRepository : IWebAuthenticationRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<WebAuthenticationRepository> _logger;

    public WebAuthenticationRepository(ApplicationDbContext dbContext, ILogger<WebAuthenticationRepository> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Administrator?> ValidateAdministratorAsync(string email, string password)
    {
        var administrator = await _dbContext.Administrators.FirstOrDefaultAsync(x => x.Email == email);

        if (administrator == null || !PasswordHelper.VerifyPassword(password, administrator.Password))
        {
            return null;
        }

        return administrator;
    }

    public async Task<bool> SignInAsync(HttpContext httpContext, Administrator administrator)
    {
        try
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, administrator.Id.ToString()),
                new Claim(ClaimTypes.Name, administrator.Name),
                new Claim(ClaimTypes.Email, administrator.Email)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddHours(1)
            };

            await httpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties
            );

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar sessão de login para o usuário: {@Administrator}", administrator);
            return false;
        }
    }

    public async Task SignOutAsync(HttpContext httpContext)
    {
        try
        {
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao realizar logout.");
            throw;
        }
    }
}