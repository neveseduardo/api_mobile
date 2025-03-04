using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApi.Models;
using WebApi.Repositories;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.Dto;
using WebApi.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using WebApi.Database;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Falah ao capturar usuário autenticado");
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
        catch (System.Exception exception)
        {
            _logger.LogError(exception, "Falha ao capturar address viewmodel");
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

            return StatusCode(200, new HttpDefaultResponse<AddressViewModel?>(viewModel));
        }
        catch (System.Exception exception)
        {
            _logger.LogError(exception, "Erro ao capturar endereço do usuário");
            return StatusCode(400, new HttpDefaultResponse<object[]>(Array.Empty<object>(), "Falha ao capturar endereço", false));
        }
    }

    [HttpPost("endereco")]
    public async Task<ActionResult<AddressViewModel?>> AddAddressAsync([FromBody] CreateAddressDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(422, new HttpDefaultResponse<ModelStateDictionary>(ModelState, "Formulário inválido", false));
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

            return StatusCode(201, new HttpDefaultResponse<AddressViewModel?>(viewModel, "Dados cadastrados com sucesso"));
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Falha ao adicionar item");
            return StatusCode(400, new HttpDefaultResponse<object[]>(Array.Empty<object>(), "Falha ao cadastrar endereço", false));
        }
    }

    [HttpPut("endereco")]
    public async Task<ActionResult> UpdateAddressAsync([FromBody] UpdateAddressDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(422, new HttpDefaultResponse<ModelStateDictionary>(ModelState, "Formulário inválido", false));
            }

            var user = await GetAuthenticatedUserAsync();

            if (user == null)
            {
                return StatusCode(404, new HttpDefaultResponse<int[]>(Array.Empty<int>(), "Usuário não contrado", false));
            }

            var address = await _context.Addresses.FirstOrDefaultAsync(u => u.Id == user.AddressId);

            if (address == null)
            {
                return StatusCode(404, new HttpDefaultResponse<int[]>(Array.Empty<int>(), "Endereço não contrado", false));
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

            return StatusCode(200, new HttpDefaultResponse<int[]?>(Array.Empty<int>(), "Dados atualizados com sucesso"));
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Falha ao adicionar item");
            return StatusCode(400, new HttpDefaultResponse<object[]>(Array.Empty<object>(), "Falha ao atualizar endereço", false));
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
                return StatusCode(404, new HttpDefaultResponse<int[]>(Array.Empty<int>(), "Usuário não contrado", false));
            }

            var address = await _context.Addresses.FirstOrDefaultAsync(u => u.Id == user.AddressId);

            if (address == null)
            {
                return StatusCode(404, new HttpDefaultResponse<int[]>(Array.Empty<int>(), "Endereço não contrado", false));
            }

            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();

            return StatusCode(200, new HttpDefaultResponse<int[]>(Array.Empty<int>(), "Dados deletados com sucesso"));
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Falha ao adicionar item");
            return StatusCode(400, new HttpDefaultResponse<object[]>(Array.Empty<object>(), "Falha ao deletar endereço", false));
        }
    }

    [HttpPut("nome")]
    public async Task<ActionResult> AlterUserNameAsync(UpdateUserDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(422, new HttpDefaultResponse<ModelStateDictionary>(ModelState, "Formulário inválido", false));
            }

            var user = await GetAuthenticatedUserAsync();

            if (user == null)
            {
                return StatusCode(404, new HttpDefaultResponse<int[]>(Array.Empty<int>(), "Usuário não contrado", false));
            }

            user.Name = dto.Name;
            user.UpdatedAt = DateTime.UtcNow;
            _context.Entry(user).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return StatusCode(200, new HttpDefaultResponse<int[]>(Array.Empty<int>(), "Dados atualizados com sucesso"));
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Falha ao adicionar item");
            return StatusCode(400, new HttpDefaultResponse<object[]>(Array.Empty<object>(), "Falha ao alterar nome", false));
        }
    }


}