using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApi.Models;
using WebApi.Repositories.Api;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.Dto;
using WebApi.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using WebApi.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WebApi.Controllers.Api;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/v1/usuarios")]
public class ApiUsersController : ControllerBase
{
    private readonly IApiUserRepository _userRepository;
    private readonly ILogger<ApiUsersController> _logger;

    public ApiUsersController(IApiUserRepository userRepository, ILogger<ApiUsersController> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserViewModel>>> GetUsers()
    {
        _logger.LogInformation("Obter todos os clientes.");

        var users = await _userRepository.GetAllUsersAsync();
        var viewModels = users.Select(c => new UserViewModel
        {
            Id = c.Id,
            Name = c.Name,
            Email = c.Email,
            Cpf = c.Cpf,
        }).ToList();

        return Ok(viewModels);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserViewModel>> GetUser(int id)
    {
        _logger.LogInformation($"Obter cliente com ID: {id}");

        var user = await _userRepository.GetUserByIdAsync(id);
        if (user == null)
        {
            _logger.LogWarning($"Cliente com ID {id} não encontrado.");
            return NotFound();
        }

        var viewModel = new UserViewModel
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Cpf = user.Cpf,
        };

        return Ok(viewModel);
    }

    [HttpPost]
    public async Task<ActionResult<UserViewModel>> CreateUser([FromBody] CreateUserDto dto)
    {
        _logger.LogInformation("Criar um novo cliente.");

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Erros de validação encontrados ao criar cliente.");
            return BadRequest(ModelState);
        }

        var User = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            Password = PasswordHelper.HashPassword(dto.Password),
        };

        await _userRepository.AddUserAsync(User);

        var viewModel = new UserViewModel
        {
            Name = User.Name,
            Email = User.Email,
            Cpf = User.Cpf,
        };

        _logger.LogInformation($"Cliente criado com ID: {User.Id}");
        return CreatedAtAction(nameof(GetUser), new { id = User.Id }, viewModel);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto dto)
    {
        _logger.LogInformation($"Atualizar cliente com ID: {id}");

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Erros de validação encontrados ao atualizar cliente.");
            return BadRequest(ModelState);
        }

        var user = await _userRepository.GetUserByIdAsync(id);
        if (user == null)
        {
            _logger.LogWarning($"Cliente com ID {id} não encontrado.");
            return NotFound();
        }

        user.Name = dto.Name;
        user.Email = dto.Email;
        user.Cpf = dto.Cpf;

        await _userRepository.UpdateUserAsync(user);

        _logger.LogInformation($"Cliente com ID {id} atualizado com sucesso.");
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        _logger.LogInformation($"Excluir cliente com ID: {id}");

        var user = await _userRepository.GetUserByIdAsync(id);
        if (user == null)
        {
            _logger.LogWarning($"Cliente com ID {id} não encontrado.");
            return NotFound();
        }

        await _userRepository.DeleteUserAsync(id);

        _logger.LogInformation($"Cliente com ID {id} excluído com sucesso.");
        return NoContent();
    }
}