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
[Route("api/v1/agendamento-avaliacoes")]
public class AppointmentRatingController : ControllerBase
{
    private readonly IRepository<AppointmentRating> _repository;
    private readonly ILogger<AppointmentRatingController> _logger;

    public AppointmentRatingController(IRepository<AppointmentRating> repository, ILogger<AppointmentRatingController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    protected AppointmentRatingViewModel GetViewModel(AppointmentRating appointmentRating)
    {
        UserViewModel? user = null;

        if (appointmentRating.User != null)
        {
            user = new UserViewModel
            {
                Id = appointmentRating.User.Id,
                Name = appointmentRating.User.Name,
                Email = appointmentRating.User.Email,
            };
        }

        return new AppointmentRatingViewModel
        {
            Id = appointmentRating.Id,
            Rating = appointmentRating.Rating,
            Comment = appointmentRating.Comment,
            User = user,
            CreatedAt = appointmentRating.CreatedAt ?? DateTime.Now,
            UpdatedAt = appointmentRating.UpdatedAt ?? DateTime.Now,
        };
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppointmentRatingViewModel>>> GetAllAsync()
    {
        var list = await _repository.GetAllAsync();
        var viewModels = list.Select(u => GetViewModel(u)).ToList();

        return StatusCode(200, ApiHelper.Ok(viewModels));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AppointmentRatingViewModel>> GetByIdAsync([FromRoute] int id)
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
    public async Task<IActionResult> AddAsync([FromBody] CreateAppointmentRatingDto dto)
    {
        try
        {
            ModelState.ClearValidationState(nameof(dto));

            if (!TryValidateModel(dto))
            {
                return StatusCode(422, ApiHelper.UnprocessableEntity(ApiHelper.GetErrorMessages(ModelState)));
            }

            var model = new AppointmentRating
            {
                Rating = dto.Rating,
                Comment = dto.Comment,
                UserId = dto.UserId,
                AppointmentId = dto.AppointmentId,
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
    public async Task<IActionResult> UpdateAsync([FromRoute] int Id, [FromBody] UpdateAppointmentRatingDto dto)
    {
        try
        {
            ModelState.ClearValidationState(nameof(dto));

            if (!TryValidateModel(dto))
            {
                return StatusCode(422, ApiHelper.UnprocessableEntity(ApiHelper.GetErrorMessages(ModelState)));
            }

            var model = await _repository.GetByIdAsync(Id);

            if (model == null)
            {
                return StatusCode(404, ApiHelper.NotFound());
            }

            model.Rating = dto.Rating ?? model.Rating;
            model.Comment = dto.Comment ?? model.Comment;

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