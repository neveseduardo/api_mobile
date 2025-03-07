using Microsoft.AspNetCore.Mvc;
using WebApi.Repositories;
using WebApi.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using WebApi.Models.ViewModels;
using WebApi.Models;
using WebApi.Helpers;
using System.Text.Json;

namespace WebApi.Controllers;

[Route("api/v1/auth/admin")]
public class AdminAuthController : ControllerBase
{
    private readonly IAuthenticationRepository<Administrator> _repository;
    private readonly ILogger<AdminAuthController> _logger;

    public AdminAuthController(IAuthenticationRepository<Administrator> authRepository, ILogger<AdminAuthController> logger)
    {
        _repository = authRepository;
        _logger = logger;
    }

    protected AdministratorViewModel GetViewModel(Administrator administrator)
    {
        var viewModel = new AdministratorViewModel
        {
            Id = administrator.Id,
            Name = administrator.Name,
            Email = administrator.Email,
            CreatedAt = administrator.CreatedAt,
            UpdatedAt = administrator.UpdatedAt,
        };

        return viewModel;
    }

    [HttpPost("login")]
    public async Task<IActionResult> AuthenticateUser([FromBody] LoginDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(422, ApiHelper.UnprocessableEntity(ModelState));
            }

            var model = await _repository.ValidateUserAsync(dto.Email, dto.Password);

            if (model == null)
            {
                return StatusCode(401, ApiHelper.Unauthorized());
            }

            var accessToken = _repository.CreateToken(model);
            var refreshToken = _repository.CreateRefreshToken(model);
            var tokens = new { AccessToken = accessToken, RefreshToken = refreshToken };

            return StatusCode(200, ApiHelper.Ok(tokens));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Falha ao tentar autenticar");
            return StatusCode(500, ApiHelper.InternalServerError());
        }
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto dto)
    {
        try
        {
            if (string.IsNullOrEmpty(dto.RefreshToken))
            {
                return BadRequest("Refresh token é obrigatório");
            }

            var principal = _repository.ValidateRefreshToken(dto.RefreshToken);

            if (principal == null)
            {
                return Unauthorized("Refresh token inválido ou expirado");
            }

            var UserIdClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (UserIdClaim == null || !int.TryParse(UserIdClaim, out int UserId))
            {
                return Unauthorized("Token inválido");
            }

            var administrator = await _repository.GetUserAsync(UserId);

            if (administrator == null)
            {
                return NotFound("Usuário não encontrado");
            }

            var newAccessToken = _repository.CreateToken(administrator);
            var newRefreshToken = _repository.CreateRefreshToken(administrator);

            return StatusCode(200, new
            {
                success = true,
                message = "Login efetuado com sucesso!",
                data = new { AccessToken = newAccessToken, RefreshToken = newRefreshToken },
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return Problem(
                detail: ex.Message,
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Internal Server Error"
            );
        }
    }

    [HttpPost("logout")]
    [Authorize(Policy = "AdminPolicy")]
    public IActionResult Logout()
    {
        return Ok(new { Message = "Logout realizado com sucesso" });
    }

    [HttpGet("usuario")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> GetUserData()
    {
        try
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                return StatusCode(401, new
                {
                    success = false,
                    message = "Token inválido",
                    data = Array.Empty<object>(),
                });
            }

            var user = await _repository.GetUserAsync(userId);

            if (user == null)
            {
                return StatusCode(404, new
                {
                    success = false,
                    message = "Usuário não encontrado",
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
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Falha ao carregar dados de usuário");
            throw;
        }

    }

    [HttpPost("register")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(422, new
                {
                    success = false,
                    message = "Formulário inválido!",
                    data = ModelState,
                });
            }

            var existingUser = await _repository.FindUserByEmailAsync(dto.Email);

            if (existingUser)
            {
                return StatusCode(422, new
                {
                    success = false,
                    message = "Já existe um usuário com este email.",
                    data = Array.Empty<object>(),
                });
            }

            var user = new Administrator
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = PasswordHelper.HashPassword(dto.Password),
            };

            await _repository.CreateUserAsync(user);

            var result = await GetUserData();

            if (result is ObjectResult objectResult)
            {
                objectResult.StatusCode = 201;
            }

            return result;
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Falha ao criar usuário");
            throw;
        }

    }

}