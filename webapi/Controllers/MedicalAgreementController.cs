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
[Route("api/v1/convenios")]
public class MedicalAgreementsController : ControllerBase
{
    private readonly IRepository<MedicalAgreement> _repository;
    private readonly ILogger<MedicalAgreementsController> _logger;

    protected MedicalAgreementViewModel GetViewModel(MedicalAgreement medicalAgreement)
    {
        var viewModel = new MedicalAgreementViewModel
        {
            Id = medicalAgreement.Id,
            Name = medicalAgreement.Name,
            Provider = medicalAgreement.Provider,
            HealthPlan = medicalAgreement.HealthPlan,
            CreatedAt = medicalAgreement.CreatedAt,
            UpdatedAt = medicalAgreement.UpdatedAt,
        };

        return viewModel;
    }

    public MedicalAgreementsController(IRepository<MedicalAgreement> MedicalAgreementRepository, ILogger<MedicalAgreementsController> logger)
    {
        _repository = MedicalAgreementRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MedicalAgreementViewModel>>> GetAllAsync()
    {
        var list = await _repository.GetAllAsync();

        var viewModels = list.Select(u => GetViewModel(u)).ToList();

        return StatusCode(200, ApiHelper.Ok(viewModels));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MedicalAgreementViewModel>> GetByIdAsync(int id)
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
    public async Task<IActionResult> AddAsync([FromBody] CreateMedicalAgreementDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(422, ApiHelper.UnprocessableEntity(ModelState));
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
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateMedicalAgreementDto dto)
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
            model.Provider = dto.Provider!;

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