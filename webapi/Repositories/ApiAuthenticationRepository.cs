using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;
using WebApi.Helpers;
using WebApi.Models;
using WebApi.Database;
using WebApi.Models.Dto;

namespace WebApi.Repositories;

public class ApiAuthenticationRepository : IApiAuthenticationRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ApiAuthenticationRepository> _logger;

    public ApiAuthenticationRepository(ApplicationDbContext dBContext, IConfiguration configuration, ILogger<ApiAuthenticationRepository> logger)
    {
        _dbContext = dBContext;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<Customer?> ValidateCustomerAsync(string email, string password)
    {
        var customer = await _dbContext.Customers.FirstOrDefaultAsync(x => x.Email == email);

        if (customer == null || !PasswordHelper.VerifyPassword(password, customer.Password))
        {
            return null;
        }

        return customer;
    }

    public string CreateToken(Customer customer)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var privateKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"] ?? "");
            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(privateKey),
                SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = credentials,
                Expires = DateTime.UtcNow.AddHours(1),
                Subject = GenerateClaims(customer)
            };

            var token = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Erro ao criar o token: parâmetro nulo");
            throw new Exception("Erro ao criar o token: parâmetro nulo", ex);
        }
        catch (FormatException ex)
        {
            _logger.LogError(ex, "Erro de formato ao criar o token");
            throw new Exception("Erro de formato ao criar o token", ex);
        }
        catch (SecurityTokenException ex)
        {
            _logger.LogError(ex, "Erro ao assinar o token JWT");
            throw new Exception("Erro ao assinar o token JWT", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao criar o token");
            throw new Exception("Erro inesperado ao criar o token", ex);
        }
    }

    public string CreateRefreshToken(Customer customer)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var privateKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"] ?? "");
            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(privateKey),
                SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = credentials,
                Expires = DateTime.UtcNow.AddDays(7),
                Subject = GenerateClaims(customer)
            };

            var token = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar o refresh token");
            throw new Exception("Erro ao criar o refresh token", ex);
        }
    }

    public ClaimsPrincipal? ValidateRefreshToken(string refreshToken)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var privateKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"] ?? "");

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(privateKey)
            };

            var principal = handler.ValidateToken(refreshToken, validationParameters, out _);
            return principal;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao validar o refresh token");
            return null;
        }
    }

    private ClaimsIdentity GenerateClaims(Customer customer)
    {
        try
        {
            var ci = new ClaimsIdentity();

            ci.AddClaim(new Claim(ClaimTypes.NameIdentifier, customer.Id.ToString()));
            ci.AddClaim(new Claim(ClaimTypes.Name, customer.Name));
            ci.AddClaim(new Claim(ClaimTypes.GivenName, customer.Name));
            ci.AddClaim(new Claim(ClaimTypes.Email, customer.Email));

            return ci;
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao criar o token");
            throw;
        }
    }

    public async Task<dynamic?> GetCustomerAsync(int id)
    {
        var customer = await _dbContext.Customers.FindAsync(id);

        if (customer == null)
        {
            return null;
        }

        return new
        {
            customer.Id,
            customer.Name,
            customer.Email,
        };
    }
}