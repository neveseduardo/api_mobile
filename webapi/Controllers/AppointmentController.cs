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
[Route("api/v1/agendamentos")]
public class AppointmentController : ControllerBase
{
    private readonly IRepository<Appointment> _repository;

    private readonly ILogger<AppointmentController> _logger;

    public AppointmentController(IRepository<Appointment> repository, ILogger<AppointmentController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    protected AppointmentViewModel? GetViewModel(Appointment? appointment)
    {
        if (appointment != null)
        {
            UserViewModel? user = null;
            DoctorViewModel? doctor = null;
            AppointmentRatingViewModel? appointmentRating = null;

            if (appointment?.User != null)
            {
                user = new UserViewModel
                {
                    Id = appointment.User.Id,
                    Name = appointment.User.Name,
                    Email = appointment.User.Email,
                };
            }

            if (appointment?.Doctor != null)
            {
                doctor = new DoctorViewModel
                {
                    Id = appointment.Doctor.Id,
                    Name = appointment.Doctor.Name,
                    Email = appointment.Doctor.Email,
                };
            }

            if (appointment!.AppointmentRating != null)
            {
                appointmentRating = new AppointmentRatingViewModel
                {
                    Id = appointment.AppointmentRating.Id,
                    Rating = appointment.AppointmentRating.Rating,
                    Comment = appointment.AppointmentRating.Comment,
                };
            }

            var viewModel = new AppointmentViewModel
            {
                Id = appointment!.Id,
                Date = appointment.Date,
                Notes = appointment.Notes,
                Status = appointment.Status,
                User = user,
                Doctor = doctor,
                AppointmentRating = appointmentRating,
                CreatedAt = appointment.CreatedAt ?? DateTime.Now,
                UpdatedAt = appointment.UpdatedAt ?? DateTime.Now,
            };

            return viewModel;
        }
        return null;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppointmentViewModel>>> GetAllAsync()
    {
        var list = await _repository.GetAllAsync();
        var viewModels = list.Select(u => GetViewModel(u)).ToList();

        return StatusCode(200, ApiHelper.Ok(viewModels));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AppointmentViewModel>> GetByIdAsync([FromRoute] int id)
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
    public async Task<IActionResult> AddAsync([FromBody] CreateAppointmentDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(422, ApiHelper.UnprocessableEntity(ModelState));
            }

            var model = new Appointment
            {
                Date = dto.Date,
                Notes = dto.Notes,
                Status = dto.Status,
                UserId = dto.UserId,
                DoctorId = dto.DoctorId,
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
    public async Task<IActionResult> UpdateAsync([FromRoute] int Id, [FromBody] UpdateAppointmentDto dto)
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

            model.Date = dto.Date ?? model.Date;
            model.Notes = dto.Notes ?? model.Notes;
            model.Status = dto.Status ?? model.Status;
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