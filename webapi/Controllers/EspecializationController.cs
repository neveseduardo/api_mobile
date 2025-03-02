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

namespace WebApi.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/v1/especializacoes")]
public class EspecializationController : ControllerBase
{
    private readonly IRepository<Especialization> _repository;
    private readonly ILogger<EspecializationController> _logger;

    public EspecializationController(IRepository<Especialization> repository, ILogger<EspecializationController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EspecializationViewModel>>> GetAllAsync()
    {
        var list = await _repository.GetAllAsync();
        var viewModelList = list.Select(u => new EspecializationViewModel
        {
            Id = u.Id,
            Name = u.Name,
            Description = u.Description,
            CreatedAt = u.CreatedAt ?? DateTime.Now,
            UpdatedAt = u.UpdatedAt ?? DateTime.Now,
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
        var especialization = await _repository.GetByIdAsync(id);

        if (especialization == null)
        {
            return StatusCode(404, new
            {
                success = true,
                message = "Item não encontrado",
                data = Array.Empty<object>(),
            });
        }

        var viewModel = new EspecializationViewModel
        {
            Id = especialization.Id,
            Name = especialization.Name,
            Description = especialization.Description,
            CreatedAt = especialization.CreatedAt ?? DateTime.Now,
            UpdatedAt = especialization.UpdatedAt ?? DateTime.Now,
        };

        return StatusCode(200, new
        {
            success = true,
            message = "Dados retornados com sucesso",
            data = viewModel,
        });
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] CreateEspecializationDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var especialization = new Especialization
            {
                Name = dto.Name,
            };

            await _repository.AddAsync(especialization);

            var result = await GetByIdAsync(especialization.Id);

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
    public async Task<IActionResult> UpdateAsync([FromRoute] int Id, [FromBody] UpdateEspecializationDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var especialization = await _repository.GetByIdAsync(Id);

            if (especialization == null)
            {
                return StatusCode(404, new
                {
                    success = true,
                    message = "Item não encontrado",
                    data = Array.Empty<object>(),
                });
            }

            especialization.Name = dto.Name ?? especialization.Name;
            especialization.Description = dto.Description ?? especialization.Description;

            await _repository.UpdateAsync(especialization);

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
            var especialization = await _repository.GetByIdAsync(id);

            if (especialization == null)
            {
                return StatusCode(404, new
                {
                    success = true,
                    message = "Item não encontrado",
                    data = Array.Empty<object>(),
                });
            }

            await _repository.DeleteAsync(especialization);

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