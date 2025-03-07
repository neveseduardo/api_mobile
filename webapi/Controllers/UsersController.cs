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
using WebApi.Helpers;

namespace WebApi.Controllers;

[Authorize(Policy = "AdminPolicy")]
[ApiController]
[Route("api/v1/usuarios")]
public class UsersController : ControllerBase
{
    private readonly IRepository<User> _repository;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IRepository<User> repository, ILogger<UsersController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    protected UserViewModel GetViewModel(User user)
    {
        var viewModel = new UserViewModel
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Cpf = user.Cpf,
        };

        return viewModel;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserViewModel>>> GetAllAsync()
    {
        var list = await _repository.GetAllAsync();
        var viewModels = list.Select(u => GetViewModel(u)).ToList();

        return StatusCode(200, ApiHelper.Ok(viewModels));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserViewModel>> GetByIdAsync([FromRoute] int id)
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
    public async Task<IActionResult> AddAsync([FromBody] CreateUserDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(422, ApiHelper.UnprocessableEntity(ModelState));
            }

            var model = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Cpf = dto.Cpf,
                Password = dto.Password,
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
            _logger.LogError(ex, "Falha ao criar item");
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }

    [HttpPut("{Id}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] int Id, [FromBody] UpdateUserDto dto)
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
            model.Cpf = dto.Cpf ?? model.Cpf;
            model.UpdatedAt = DateTime.Now;


            await _repository.UpdateAsync(model);

            return StatusCode(200, ApiHelper.Ok());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Falha ao atualizar item");
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
            _logger.LogError(ex, "Falha ao deletar item");
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }
}