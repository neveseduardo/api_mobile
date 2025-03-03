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

[Authorize(Policy = "AdminPolicy")]
[ApiController]
[Route("api/v1/medicos")]
public class DoctorController : ControllerBase
{
    private readonly IRepository<Doctor> _repository;
    private readonly ILogger<DoctorController> _logger;

    public DoctorController(IRepository<Doctor> repository, ILogger<DoctorController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DoctorViewModel>>> GetAllAsync()
    {
        var list = await _repository.GetAllAsync();
        var viewModelList = list.Select(u => new DoctorViewModel
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email,
            CRM = u.CRM,
        });

        return StatusCode(200, new
        {
            success = true,
            message = "Dados retornados com sucesso",
            data = viewModelList,
        });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DoctorViewModel>> GetByIdAsync([FromRoute] int id)
    {
        var doctor = await _repository.GetByIdAsync(id);

        if (doctor == null)
        {
            return StatusCode(404, new
            {
                success = true,
                message = "Item não encontrado",
                data = Array.Empty<object>(),
            });
        }

        var viewModel = new DoctorViewModel
        {
            Id = doctor.Id,
            Name = doctor.Name,
            Email = doctor.Email,
        };

        return StatusCode(200, new
        {
            success = true,
            message = "Dados retornados com sucesso",
            data = viewModel,
        });
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] CreateDoctorDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var doctor = new Doctor
            {
                Name = dto.Name,
                Email = dto.Email,
                CPF = dto.CPF,
                CRM = dto.CRM,
                EspecializationId = dto.EspecializationId,
            };

            await _repository.AddAsync(doctor);

            var result = await GetByIdAsync(doctor.Id);

            if (result.Result is ObjectResult objectResult)
            {
                objectResult.StatusCode = 201;
                return objectResult;
            }

            return StatusCode(201, result.Value);
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
    public async Task<IActionResult> UpdateAsync([FromRoute] int Id, [FromBody] UpdateDoctorDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var doctor = await _repository.GetByIdAsync(Id);

            if (doctor == null)
            {
                return StatusCode(404, new
                {
                    success = true,
                    message = "Item não encontrado",
                    data = Array.Empty<object>(),
                });
            }

            doctor.Name = dto.Name ?? doctor.Name;
            doctor.Email = dto.Email ?? doctor.Email;
            doctor.CPF = dto.CPF ?? doctor.CPF;

            await _repository.UpdateAsync(doctor);

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
            var doctor = await _repository.GetByIdAsync(id);

            if (doctor == null)
            {
                return StatusCode(404, new
                {
                    success = true,
                    message = "Item não encontrado",
                    data = Array.Empty<object>(),
                });
            }

            await _repository.DeleteAsync(doctor);

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