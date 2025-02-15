using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WebApi.Helpers;
using WebApi.Repositories;
using WebApi.Models;
using WebApi.Models.Dto;
using WebApi.Models.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WebApi.Controllers.Api;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/v1/usuarios")]
public class ApiUserController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<ApiUserController> _logger;

    public ApiUserController(IUserRepository userRepository, ILogger<ApiUserController> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IEnumerable<UserViewModel>> GetUsers()
    {
        _logger.LogInformation("Obter todos os usuários.");

        var users = await _userRepository.GetUsersAsync();
        var viewModel = users.Select(u => new UserViewModel
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email,
            Roles = u.Roles,
        });

        return viewModel;
    }

    [HttpGet("{Id}")]
    public async Task<IActionResult> GetUser([FromRoute] int Id)
    {
        var user = await _userRepository.GetUserByIdAsync(Id);

        if (user == null)
        {
            _logger.LogWarning($"Usuário com Id {Id} não encontrado.");
            return NotFound();
        }

        var viewModel = new UserViewModel
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Roles = user.Roles,
        };

        return Ok(viewModel);
    }


    [HttpPost]
    public async Task<IActionResult> StoreUser([FromBody] CreateUserDto dto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Erros de valIdação encontrados ao criar usuário.");
            return BadRequest(ModelState);
        }

        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            Password = PasswordHelper.HashPassword(dto.Password),
            Roles = dto.Roles,
        };

        user.Password = PasswordHelper.HashPassword(user.Password);

        await _userRepository.AddUserAsync(user);

        return CreatedAtAction("GetUser", new { Id = user.Id }, user);
    }

    [HttpDelete("{Id}")]
    public async Task<IActionResult> DeleteUser([FromRoute] int Id)
    {
        var user = await _userRepository.DeleteUserAsync(Id);

        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpPut("{Id}")]
    public async Task<IActionResult> UpdateUser([FromRoute] int Id, [FromBody] UpdateUserDto dto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Erros de valIdação encontrados ao atualizar usuário.");
            return BadRequest(ModelState);
        }

        var user = await _userRepository.GetUserByIdAsync(Id);

        if (user == null)
        {
            _logger.LogWarning($"Usuário com Id {Id} não encontrado.");
            return NotFound();
        }

        user.Name = dto.Name;
        user.Email = dto.Email;
        user.Roles = dto.Roles;

        var updatedUser = await _userRepository.UpdateUserAsync(Id, user);

        if (updatedUser == null)
        {
            return NotFound();
        }

        return Ok(updatedUser);
    }
}