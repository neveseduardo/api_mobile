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

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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

    protected AppointmentRatingViewModel? GetViewModel(AppointmentRating appointmentRating)
    {
        if (appointmentRating != null)
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
        return null;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppointmentRatingViewModel>>> GetAllAsync()
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
        var appointmentRating = await _repository.GetByIdAsync(id);

        if (appointmentRating == null)
        {
            return StatusCode(404, new
            {
                success = true,
                message = "Item não encontrado",
                data = Array.Empty<object>(),
            });
        }

        var viewModel = GetViewModel(appointmentRating);

        return StatusCode(200, new
        {
            success = true,
            message = "Dados retornados com sucesso",
            data = viewModel,
        });
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] CreateAppointmentRatingDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appointmentRating = new AppointmentRating
            {
                Rating = dto.Rating,
                Comment = dto.Comment,
                UserId = dto.UserId,
            };

            await _repository.AddAsync(appointmentRating);

            var result = await GetByIdAsync(appointmentRating.Id);

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
    public async Task<IActionResult> UpdateAsync([FromRoute] int Id, [FromBody] UpdateAppointmentRatingDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appointmentRating = await _repository.GetByIdAsync(Id);

            if (appointmentRating == null)
            {
                return StatusCode(404, new
                {
                    success = true,
                    message = "Item não encontrado",
                    data = Array.Empty<object>(),
                });
            }

            appointmentRating.Rating = dto.Rating ?? appointmentRating.Rating;
            appointmentRating.Comment = dto.Comment ?? appointmentRating.Comment;

            await _repository.UpdateAsync(appointmentRating);

            return StatusCode(204, new
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
            var appointmentRating = await _repository.GetByIdAsync(id);

            if (appointmentRating == null)
            {
                return StatusCode(404, new
                {
                    success = true,
                    message = "Item não encontrado",
                    data = Array.Empty<object>(),
                });
            }

            await _repository.DeleteAsync(appointmentRating);

            return StatusCode(204, new
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