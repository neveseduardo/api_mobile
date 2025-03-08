using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Models.Dto;
using WebApi.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using WebApi.Database;
using System.Security.Claims;
using WebApi.Helpers;

namespace WebApi.Controllers;

[Authorize(Policy = "UserPolicy")]
[ApiController]
[Route("api/v1/public")]
public class PublicController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<PublicController> _logger;

    public PublicController(ApplicationDbContext context, ILogger<PublicController> logger)
    {
        _context = context;
        _logger = logger;
    }

    protected async Task<User?> GetAuthenticatedUserAsync()
    {
        try
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                return null;
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    protected AddressViewModel? GetAddressViewModelAsync(Address? address)
    {
        try
        {
            if (address != null)
            {
                var viewModel = new AddressViewModel
                {
                    Id = address.Id,
                    Logradouro = address.Logradouro,
                    Cep = address.Cep,
                    Bairro = address.Bairro,
                    Cidade = address.Cidade,
                    Estado = address.Estado,
                    Pais = address.Pais,
                    Numero = address.Numero,
                    Complemento = address.Complemento
                };

                return viewModel;
            }

            return null;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);
            throw;
        }
    }

    [HttpGet("endereco")]
    public async Task<ActionResult<IEnumerable<AddressViewModel>>> GetUserAddressAsync()
    {
        try
        {
            var user = await GetAuthenticatedUserAsync();
            AddressViewModel? viewModel = null;

            if (user != null)
            {
                var address = await _context.Addresses.FirstOrDefaultAsync(u => u.Id == user.AddressId);

                viewModel = GetAddressViewModelAsync(address);
            }

            return StatusCode(200, ApiHelper.Ok(viewModel!));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }

    [HttpPost("endereco")]
    public async Task<ActionResult<AddressViewModel?>> AddAddressAsync([FromBody] CreateAddressDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(422, ApiHelper.UnprocessableEntity(ModelState));
            }

            var address = new Address
            {
                Logradouro = dto.Logradouro,
                Cep = dto.Cep,
                Bairro = dto.Bairro,
                Cidade = dto.Cidade,
                Estado = dto.Estado,
                Pais = dto.Pais,
                Numero = dto.Numero,
                Complemento = dto.Complemento
            };

            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();

            var user = await GetAuthenticatedUserAsync();

            if (user != null)
            {
                user.AddressId = address.Id;
                user.UpdatedAt = DateTime.UtcNow;
                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }

            var viewModel = GetAddressViewModelAsync(address);

            return StatusCode(201, ApiHelper.Ok(viewModel!));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }

    [HttpPut("endereco")]
    public async Task<ActionResult> UpdateAddressAsync([FromBody] UpdateAddressDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(422, ApiHelper.UnprocessableEntity(ModelState));
            }

            var user = await GetAuthenticatedUserAsync();

            if (user == null)
            {
                return StatusCode(404, ApiHelper.NotFound());
            }

            var address = await _context.Addresses.FirstOrDefaultAsync(u => u.Id == user.AddressId);

            if (address == null)
            {
                return StatusCode(404, ApiHelper.NotFound());
            }

            address.Logradouro = dto.Logradouro ?? address.Logradouro;
            address.Cep = dto.Cep ?? address.Cep;
            address.Bairro = dto.Bairro ?? address.Bairro;
            address.Cidade = dto.Cidade ?? address.Cidade;
            address.Estado = dto.Estado ?? address.Estado;
            address.Pais = dto.Pais ?? address.Pais;
            address.Numero = dto.Numero ?? address.Numero;
            address.Complemento = dto.Complemento ?? address.Complemento;

            address.UpdatedAt = DateTime.UtcNow;
            _context.Entry(address).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return StatusCode(200, ApiHelper.Ok(address));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }

    [HttpDelete("endereco")]
    public async Task<ActionResult> DeleteAddressAsync()
    {
        try
        {
            var user = await GetAuthenticatedUserAsync();

            if (user == null)
            {
                return StatusCode(404, ApiHelper.NotFound());
            }

            var address = await _context.Addresses.FirstOrDefaultAsync(u => u.Id == user.AddressId);

            if (address == null)
            {
                return StatusCode(404, ApiHelper.NotFound());
            }

            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();

            return StatusCode(200, ApiHelper.Ok());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }

    [HttpPut("nome")]
    public async Task<ActionResult> AlterUserNameAsync(UpdateUserDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(404, ApiHelper.NotFound());
            }

            var user = await GetAuthenticatedUserAsync();

            if (user == null)
            {
                return StatusCode(404, ApiHelper.NotFound());
            }

            user.Name = dto.Name;
            user.UpdatedAt = DateTime.UtcNow;
            _context.Entry(user).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return StatusCode(200, ApiHelper.Ok());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }


}