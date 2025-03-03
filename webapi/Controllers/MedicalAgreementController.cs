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
[Route("api/v1/convenios")]
public class MedicalAgreementsController : ControllerBase
{
    private readonly IRepository<MedicalAgreement> _repository;
    private readonly ILogger<MedicalAgreementsController> _logger;

    public MedicalAgreementsController(IRepository<MedicalAgreement> MedicalAgreementRepository, ILogger<MedicalAgreementsController> logger)
    {
        _repository = MedicalAgreementRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MedicalAgreementViewModel>>> GetAllAsync()
    {
        var list = await _repository.GetAllAsync();

        var viewModels = list.Select(a => new MedicalAgreementViewModel
        {
            Id = a.Id,
            Name = a.Name,
            Provider = a.Provider,
            HealthPlan = a.HealthPlan
        }).ToList();

        return StatusCode(200, new
        {
            success = true,
            message = "Dados retornados com sucesso",
            data = viewModels,
        });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MedicalAgreementViewModel>> GetByIdAsync(int id)
    {
        var model = await _repository.GetByIdAsync(id);

        if (model == null)
        {
            return StatusCode(404, new
            {
                success = true,
                message = "Item não encontrado",
                data = Array.Empty<object>(),
            });
        }

        var viewModel = new MedicalAgreementViewModel
        {
            Id = model.Id,
            Name = model.Name,
            Provider = model.Provider,
            HealthPlan = model.HealthPlan
        };

        return StatusCode(200, new
        {
            success = true,
            message = "Dados retornados com sucesso",
            data = viewModel,
        }); ;
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] CreateMedicalAgreementDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var MedicalAgreement = new MedicalAgreement
            {
                Name = dto.Name,
                Provider = dto.Provider,
            };

            await _repository.AddAsync(MedicalAgreement);

            var result = await GetByIdAsync(MedicalAgreement.Id);

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
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateMedicalAgreementDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var model = await _repository.GetByIdAsync(id);

            if (model == null)
            {
                return StatusCode(404, new
                {
                    success = true,
                    message = "Item não encontrado",
                    data = Array.Empty<object>(),
                });
            }

            model.Name = dto.Name!;
            model.Provider = dto.Provider!;

            await _repository.UpdateAsync(model);

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
            var model = await _repository.GetByIdAsync(id);

            if (model == null)
            {
                return StatusCode(404, new
                {
                    success = true,
                    message = "Item não encontrado",
                    data = Array.Empty<object>(),
                });
            }

            await _repository.DeleteAsync(model);

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