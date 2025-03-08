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
using WebApi.Helpers;

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

        return StatusCode(200, ApiHelper.Ok(viewModels));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MedicalExamViewModel>> GetByIdAsync(int id)
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
    public async Task<ActionResult> AddAsync([FromBody] CreateMedicalExamDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(422, ApiHelper.UnprocessableEntity(ModelState));
            }

            var model = new MedicalExam
            {
                Name = dto.Name,
                Description = dto.Description
            };

            await _repository.AddAsync(model);

            var result = await GetByIdAsync(model.Id);

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
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateMedicalExamDto dto)
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

            model.Name = dto.Name ?? model.Name;
            model.Description = dto.Description ?? model.Description;
            model.Active = dto.Active ?? model.Active;
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