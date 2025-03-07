using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApi.Models;
using WebApi.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.Dto;
using WebApi.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WebApi.Controllers;

[Authorize(Policy = "AdminPolicy")]
[ApiController]
[Route("api/v1/exames")]
public class MedicalExamController : ControllerBase
{
    private readonly IRepository<MedicalExam> _repository;
    private readonly ILogger<MedicalExamController> _logger;

    public MedicalExamController(IRepository<MedicalExam> MedicalExamRepository, ILogger<MedicalExamController> logger)
    {
        _repository = MedicalExamRepository;
        _logger = logger;
    }

    protected MedicalExamViewModel GetViewModel(MedicalExam model)
    {
        var viewModel = new MedicalExamViewModel
        {
            Id = model.Id,
            Name = model.Name,
            Description = model.Description,
            CreatedAt = model.CreatedAt,
            UpdatedAt = model.UpdatedAt,
        };

        return viewModel;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MedicalExamViewModel>>> GetAllAsync()
    {
        var list = await _repository.GetAllAsync();

        var viewModels = list.Select(a => GetViewModel(a)).ToList();

        return StatusCode(200, new
        {
            success = true,
            message = "Dados retornados com sucesso",
            data = viewModels,
        });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MedicalExamViewModel>> GetByIdAsync(int id)
    {
        var medicalExam = await _repository.GetByIdAsync(id);

        if (medicalExam == null)
        {
            return StatusCode(404, new
            {
                success = true,
                message = "Item não encontrado",
                data = Array.Empty<object>(),
            });
        }

        var viewModel = GetViewModel(medicalExam);

        return StatusCode(200, new
        {
            success = true,
            message = "Dados retornados com sucesso",
            data = viewModel,
        }); ;
    }

    [HttpPost]
    public async Task<ActionResult> AddAsync([FromBody] CreateMedicalExamDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var medicalExam = new MedicalExam
            {
                Name = dto.Name,
                Description = dto.Description
            };

            await _repository.AddAsync(medicalExam);

            var result = await GetByIdAsync(medicalExam.Id);

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

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateMedicalExamDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var medicalExam = await _repository.GetByIdAsync(id);

            if (medicalExam == null)
            {
                return StatusCode(404, new
                {
                    success = true,
                    message = "Item não encontrado",
                    data = Array.Empty<object>(),
                });
            }

            medicalExam.Name = dto.Name ?? medicalExam.Name;
            medicalExam.Description = dto.Description ?? medicalExam.Description;
            medicalExam.Active = dto.Active ?? medicalExam.Active;

            await _repository.UpdateAsync(medicalExam);

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
                success = true,
                message = "Falha ao atualizar item",
                trace = ex.Message,
                data = Array.Empty<object>(),
            });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        try
        {
            var medicalExam = await _repository.GetByIdAsync(id);

            if (medicalExam == null)
            {
                return StatusCode(404, new
                {
                    success = true,
                    message = "Item não encontrado",
                    data = Array.Empty<object>(),
                });
            }

            await _repository.DeleteAsync(medicalExam);

            return StatusCode(200, new
            {
                success = true,
                message = "Objeto deletado com sucesso",
                data = Array.Empty<object>(),
            });
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Falha ao atualizar item");

            return StatusCode(400, new
            {
                success = true,
                message = "Falha ao atualizar item",
                trace = ex.Message,
                data = Array.Empty<object>(),
            });
        }
    }
}