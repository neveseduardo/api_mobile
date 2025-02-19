using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WebApi.Helpers;
using WebApi.Repositories.Api;
using WebApi.Models;
using WebApi.Models.Dto;
using WebApi.Models.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WebApi.Controllers.Api;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/v1/administradores")]
public class ApiAdministratorController : Controller
{
    private readonly IApiAdministratorRepository _AdministratorRepository;
    private readonly ILogger<ApiAdministratorController> _logger;

    public ApiAdministratorController(IApiAdministratorRepository AdministratorRepository, ILogger<ApiAdministratorController> logger)
    {
        _AdministratorRepository = AdministratorRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IEnumerable<AdministratorViewModel>> GetAdministrators()
    {
        _logger.LogInformation("Obter todos os usuários.");

        var Administrators = await _AdministratorRepository.GetAdministratorsAsync();
        var viewModel = Administrators.Select(u => new AdministratorViewModel
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email,
        });

        return viewModel;
    }

    [HttpGet("{Id}")]
    public async Task<IActionResult> GetAdministrator([FromRoute] int Id)
    {
        var Administrator = await _AdministratorRepository.GetAdministratorByIdAsync(Id);

        if (Administrator == null)
        {
            _logger.LogWarning($"Usuário com Id {Id} não encontrado.");
            return NotFound();
        }

        var viewModel = new AdministratorViewModel
        {
            Id = Administrator.Id,
            Name = Administrator.Name,
            Email = Administrator.Email,
        };

        return Ok(viewModel);
    }


    [HttpPost]
    public async Task<IActionResult> StoreAdministrator([FromBody] CreateAdministratorDto dto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Erros de valIdação encontrados ao criar usuário.");
            return BadRequest(ModelState);
        }

        var Administrator = new Administrator
        {
            Name = dto.Name,
            Email = dto.Email,
            Password = PasswordHelper.HashPassword(dto.Password),
        };

        Administrator.Password = PasswordHelper.HashPassword(Administrator.Password);

        await _AdministratorRepository.AddAdministratorAsync(Administrator);

        return CreatedAtAction("GetAdministrator", new { Id = Administrator.Id }, Administrator);
    }

    [HttpDelete("{Id}")]
    public async Task<IActionResult> DeleteAdministrator([FromRoute] int Id)
    {
        var Administrator = await _AdministratorRepository.DeleteAdministratorAsync(Id);

        if (Administrator == null)
        {
            return NotFound();
        }

        return Ok(Administrator);
    }

    [HttpPut("{Id}")]
    public async Task<IActionResult> UpdateAdministrator([FromRoute] int Id, [FromBody] UpdateAdministratorDto dto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Erros de valIdação encontrados ao atualizar usuário.");
            return BadRequest(ModelState);
        }

        var Administrator = await _AdministratorRepository.GetAdministratorByIdAsync(Id);

        if (Administrator == null)
        {
            _logger.LogWarning($"Usuário com Id {Id} não encontrado.");
            return NotFound();
        }

        Administrator.Name = dto.Name;
        Administrator.Email = dto.Email;

        var updatedAdministrator = await _AdministratorRepository.UpdateAdministratorAsync(Id, Administrator);

        if (updatedAdministrator == null)
        {
            return NotFound();
        }

        return Ok(updatedAdministrator);
    }
}