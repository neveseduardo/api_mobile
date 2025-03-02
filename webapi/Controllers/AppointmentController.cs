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

// [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
                    CreatedAt = appointment.AppointmentRating.CreatedAt ?? DateTime.Now,
                    UpdatedAt = appointment.AppointmentRating.UpdatedAt ?? DateTime.Now,
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
        var viewModelList = list.Select(u => GetViewModel(u)).ToList();

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
        var appointment = await _repository.GetByIdAsync(id);

        if (appointment == null)
        {
            return StatusCode(404, new
            {
                success = true,
                message = "Item não encontrado",
                data = Array.Empty<object>(),
            });
        }

        var viewModel = GetViewModel(appointment);

        return StatusCode(200, new
        {
            success = true,
            message = "Dados retornados com sucesso",
            data = viewModel,
        });
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] CreateAppointmentDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appointment = new Appointment
            {
                Date = dto.Date,
                Notes = dto.Notes,
                Status = dto.Status,
                UserId = dto.UserId,
                DoctorId = dto.DoctorId,
            };

            await _repository.AddAsync(appointment);

            var result = await GetByIdAsync(appointment.Id);

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
    public async Task<IActionResult> UpdateAsync([FromRoute] int Id, [FromBody] UpdateAppointmentDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appointment = await _repository.GetByIdAsync(Id);

            if (appointment == null)
            {
                return StatusCode(404, new
                {
                    success = true,
                    message = "Item não encontrado",
                    data = Array.Empty<object>(),
                });
            }

            appointment.Date = dto.Date ?? appointment.Date;
            appointment.Notes = dto.Notes ?? appointment.Notes;
            appointment.Status = dto.Status ?? appointment.Status;

            await _repository.UpdateAsync(appointment);

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
            var appointment = await _repository.GetByIdAsync(id);

            if (appointment == null)
            {
                return StatusCode(404, new
                {
                    success = true,
                    message = "Item não encontrado",
                    data = Array.Empty<object>(),
                });
            }

            await _repository.DeleteAsync(appointment);

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