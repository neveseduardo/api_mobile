using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WebApi.Helpers;
using WebApi.Repositories;
using WebApi.Models;
using WebApi.Models.Dto;
using WebApi.Models.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WebApi.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/v1/administradores")]
public class AdministratorController : Controller
{
    private readonly IRepository<Administrator> _repository;
    private readonly ILogger<AdministratorController> _logger;

    public AdministratorController(IRepository<Administrator> AdministratorRepository, ILogger<AdministratorController> logger)
    {
        _repository = AdministratorRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AdministratorViewModel>>> GetAllAsync()
    {
        var list = await _repository.GetAllAsync();
        var viewModelList = list.Select(u => new AdministratorViewModel
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email,
        });

        return StatusCode(200, new
        {
            success = true,
            message = "Dados retornados com sucesso",
            data = viewModelList,
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        var administrator = await _repository.GetByIdAsync(id);

        if (administrator == null)
        {
            return StatusCode(404, new
            {
                success = true,
                message = "Item não encontrado",
                data = Array.Empty<object>(),
            });
        }

        var viewModel = new AdministratorViewModel
        {
            Id = administrator.Id,
            Name = administrator.Name,
            Email = administrator.Email,
        };

        return StatusCode(200, new
        {
            success = true,
            message = "Dados retornados com sucesso",
            data = viewModel,
        });
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] CreateAdministratorDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var administrator = new Administrator
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = dto.Password,
            };

            await _repository.AddAsync(administrator);

            var result = await GetByIdAsync(administrator.Id);

            if (result is ObjectResult objectResult)
            {
                objectResult.StatusCode = 201;
            }

            return result;
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Falha ao criar item");

            return StatusCode(400, new
            {
                success = true,
                message = "Falha ao criar item",
                trace = ex.Message,
                data = Array.Empty<object>(),
            });
        }
    }

    [HttpPut("{Id}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] int Id, [FromBody] UpdateAdministratorDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var administrator = await _repository.GetByIdAsync(Id);

            if (administrator == null)
            {
                return StatusCode(404, new
                {
                    success = true,
                    message = "Item não encontrado",
                    data = Array.Empty<object>(),
                });
            }

            administrator.Name = dto.Name;
            administrator.Email = dto.Email;

            await _repository.UpdateAsync(administrator);

            return StatusCode(200, new
            {
                success = true,
                message = "Dados atualizados com sucesso",
                data = Array.Empty<object>(),
            });
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Falha ao atualizar item");

            return StatusCode(400, new
            {
                success = false,
                message = "Falha ao atualizar item",
                trace = ex.Message,
                data = Array.Empty<object>(),
            });
        }

    }

    [HttpDelete("{Id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        try
        {
            var administrator = await _repository.GetByIdAsync(id);

            if (administrator == null)
            {
                return StatusCode(404, new
                {
                    success = true,
                    message = "Item não encontrado",
                    data = Array.Empty<object>(),
                });
            }

            await _repository.DeleteAsync(administrator);

            return StatusCode(200, new
            {
                success = true,
                message = "Dados deletados com sucesso",
                data = Array.Empty<object>(),
            });
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Falha ao deletar item");

            return StatusCode(400, new
            {
                success = false,
                message = "Falha ao deletar item",
                trace = ex.Message,
                data = Array.Empty<object>(),
            });
        }
    }
}