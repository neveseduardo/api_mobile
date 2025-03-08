using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Repositories;
using WebApi.Models.Dto;
using WebApi.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using WebApi.Helpers;

namespace WebApi.Controllers;

[Authorize(Policy = "AdminPolicy")]
[ApiController]
[Route("api/v1/planos")]
public class HealthPlansController : ControllerBase
{
    private readonly IRepository<HealthPlan> _repository;
    private readonly ILogger<HealthPlansController> _logger;

    public HealthPlansController(IRepository<HealthPlan> HealthPlanRepository, ILogger<HealthPlansController> logger)
    {
        _repository = HealthPlanRepository;
        _logger = logger;
    }

    protected HealthPlanViewModel GetViewModel(HealthPlan healthPlan)
    {
        var viewModel = new HealthPlanViewModel
        {
            Id = healthPlan.Id,
            Name = healthPlan.Name,
            Coverage = healthPlan.Coverage,
            MedicalAgreements = healthPlan.MedicalAgreements,
            CreatedAt = healthPlan.CreatedAt,
            UpdatedAt = healthPlan.UpdatedAt,
        };

        return viewModel;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<HealthPlanViewModel>>> GetAllAsync()
    {
        var list = await _repository.GetAllAsync();

        var viewModels = list.Select(u => GetViewModel(u)).ToList();

        return StatusCode(200, ApiHelper.Ok(viewModels));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IEnumerable<HealthPlanViewModel>>> GetByIdAsync(int id)
    {
        var model = await _repository.GetByIdAsync(id);

        if (model == null)
        {
            return StatusCode(404, ApiHelper.NotFound());
        }

        var viewModel = GetViewModel(model);

        return StatusCode(200, ApiHelper.Ok(viewModel));
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] CreateHealthPlanDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(422, ApiHelper.UnprocessableEntity(ModelState));
            }

            var HealthPlan = new HealthPlan
            {
                Name = dto.Name,
                Coverage = dto.Coverage,
            };

            await _repository.AddAsync(HealthPlan);

            var result = await GetByIdAsync(HealthPlan.Id);

            if (result.Result is ObjectResult objectResult)
            {
                objectResult.StatusCode = 201;
                return objectResult;
            }

            return StatusCode(201, result.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateHealthPlanDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(422, ApiHelper.UnprocessableEntity(ModelState));
            }

            var model = await _repository.GetByIdAsync(id);

            if (model == null)
            {
                return StatusCode(404, ApiHelper.NotFound());
            }

            model.Name = dto.Name!;
            model.Coverage = dto.Coverage!;
            model.UpdatedAt = DateTime.Now;

            await _repository.UpdateAsync(model);

            return StatusCode(200, ApiHelper.Ok());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
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
                return StatusCode(404, ApiHelper.NotFound());
            }

            await _repository.DeleteAsync(model);

            return StatusCode(200, ApiHelper.Ok());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }
}