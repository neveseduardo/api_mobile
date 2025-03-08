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

    protected DoctorViewModel GetViewModel(Doctor doctor)
    {
        EspecializationViewModel? especializationViewModel = null;

        if (doctor.Especialization != null)
        {
            especializationViewModel = new EspecializationViewModel
            {
                Id = doctor.Especialization.Id,
                Name = doctor.Especialization.Name,
                Description = doctor.Especialization.Description,
                CreatedAt = doctor.Especialization.CreatedAt,
                UpdatedAt = doctor.Especialization.UpdatedAt,
            };
        }

        var viewModel = new DoctorViewModel
        {
            Id = doctor.Id,
            Name = doctor.Name,
            Email = doctor.Email,
            CRM = doctor.CRM,
            Especialization = especializationViewModel,
            CreatedAt = doctor.CreatedAt,
            UpdatedAt = doctor.UpdatedAt,
        };

        return viewModel;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DoctorViewModel>>> GetAllAsync()
    {
        var list = await _repository.GetAllAsync();
        var viewModels = list.Select(u => GetViewModel(u)).ToList();

        return StatusCode(200, ApiHelper.Ok(viewModels));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DoctorViewModel>> GetByIdAsync([FromRoute] int id)
    {
        var model = await _repository.GetByIdAsync(id);

        if (model == null)
        {
            return StatusCode(404, ApiHelper.NotFound());
        }

        var viewModel = GetViewModel(model);

        return StatusCode(200, ApiHelper.Ok(viewModel!));
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] CreateDoctorDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(422, ApiHelper.UnprocessableEntity(ModelState));
            }

            var model = new Doctor
            {
                Name = dto.Name,
                Email = dto.Email,
                CPF = dto.CPF,
                CRM = dto.CRM,
                EspecializationId = dto.EspecializationId,
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

    [HttpPut("{Id}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] int Id, [FromBody] UpdateDoctorDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(422, ApiHelper.UnprocessableEntity(ModelState));
            }

            var model = await _repository.GetByIdAsync(Id);

            if (model == null)
            {
                return StatusCode(404, ApiHelper.NotFound());
            }

            model.Name = dto.Name ?? model.Name;
            model.Email = dto.Email ?? model.Email;
            model.CPF = dto.CPF ?? model.CPF;
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

    [HttpDelete("{Id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
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