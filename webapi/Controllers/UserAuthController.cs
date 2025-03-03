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

[Route("api/v1/auth/user")]
public class UserAuthController : ControllerBase
{
    private readonly IAuthenticationRepository<User> _repository;
    private readonly ILogger<UserAuthController> _logger;

    public UserAuthController(IAuthenticationRepository<User> authRepository, ILogger<UserAuthController> logger)
    {
        _repository = authRepository;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> AuthenticateUser([FromBody] LoginDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (dto == null || string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.Password))
            {
                return BadRequest("Invalid client request");
            }

            var user = await _repository.ValidateUserAsync(dto.Email, dto.Password);

            if (user == null)
            {
                return Unauthorized();
            }

            var accessToken = _repository.CreateToken(user);
            var refreshToken = _repository.CreateRefreshToken(user);

            return StatusCode(200, new
            {
                success = true,
                message = "Login efetuado com sucesso!",
                data = new { AccessToken = accessToken, RefreshToken = refreshToken },
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

            var user = await _repository.GetUserAsync(UserId);

            if (user == null)
            {
                return NotFound("Usuário não encontrado");
            }

            var newAccessToken = _repository.CreateToken(user);
            var newRefreshToken = _repository.CreateRefreshToken(user);

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
    [Authorize(Policy = "UserPolicy")]
    public IActionResult Logout()
    {
        return Ok(new { Message = "Logout realizado com sucesso" });
    }

    [HttpGet("usuario")]
    [Authorize(Policy = "UserPolicy")]
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

            var viewModel = new UserViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Cpf = user.Cpf,
            };


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
                _logger.LogWarning("Erros de validação encontrados ao criar cliente.");
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

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Cpf = dto.Cpf,
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