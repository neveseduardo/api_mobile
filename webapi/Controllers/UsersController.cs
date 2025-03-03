﻿using Microsoft.AspNetCore.Mvc;
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

    protected UserViewModel? GetViewModel(User? user)
    {
        if (user != null)
        {
            var viewModel = new UserViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
            };

            return viewModel;
        }
        return null;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserViewModel>>> GetAllAsync()
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
    public async Task<ActionResult<UserViewModel>> GetByIdAsync([FromRoute] int id)
    {
        var user = await _repository.GetByIdAsync(id);

        if (user == null)
        {
            return StatusCode(404, new
            {
                success = true,
                message = "Item não encontrado",
                data = Array.Empty<object>(),
            });
        }

        var viewModel = GetViewModel(user);

        return StatusCode(200, new
        {
            success = true,
            message = "Dados retornados com sucesso",
            data = viewModel,
        });
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] CreateUserDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = dto.Password,
            };

            await _repository.AddAsync(user);

            var result = await GetByIdAsync(user.Id);

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

    [HttpPut("{Id}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] int Id, [FromBody] UpdateUserDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _repository.GetByIdAsync(Id);

            if (user == null)
            {
                return StatusCode(404, new
                {
                    success = true,
                    message = "Item não encontrado",
                    data = Array.Empty<object>(),
                });
            }

            user.Name = dto.Name;
            user.Email = dto.Email;

            await _repository.UpdateAsync(user);

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
            var user = await _repository.GetByIdAsync(id);

            if (user == null)
            {
                return StatusCode(404, new
                {
                    success = true,
                    message = "Item não encontrado",
                    data = Array.Empty<object>(),
                });
            }

            await _repository.DeleteAsync(user);

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