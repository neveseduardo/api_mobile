using Microsoft.AspNetCore.Mvc;
using WebApi.Repositories.Api;
using WebApi.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using WebApi.Models.ViewModels;
using WebApi.Models;
using WebApi.Helpers;
using System.Text.Json;

namespace WebApi.Controllers.Api;

[Route("api/v1/auth")]
public class ApiAuthenticationController : ControllerBase
{
    private readonly IApiAuthenticationRepository _authRepository;
    private readonly ILogger<ApiAuthenticationController> _logger;

    public ApiAuthenticationController(IApiAuthenticationRepository authRepository, ILogger<ApiAuthenticationController> logger)
    {
        _authRepository = authRepository;
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

            var user = await _authRepository.ValidateUserAsync(dto.Email, dto.Password);

            if (user == null)
            {
                return Unauthorized();
            }

            var accessToken = _authRepository.CreateToken(user);
            var refreshToken = _authRepository.CreateRefreshToken(user);

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

            var principal = _authRepository.ValidateRefreshToken(dto.RefreshToken);

            if (principal == null)
            {
                return Unauthorized("Refresh token inválido ou expirado");
            }

            var UserIdClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (UserIdClaim == null || !int.TryParse(UserIdClaim, out int UserId))
            {
                return Unauthorized("Token inválido");
            }

            var user = await _authRepository.GetUserAsync(UserId);

            if (user == null)
            {
                return NotFound("Usuário não encontrado");
            }

            var newAccessToken = _authRepository.CreateToken(user);
            var newRefreshToken = _authRepository.CreateRefreshToken(user);

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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult Logout()
    {
        return Ok(new { Message = "Logout realizado com sucesso" });
    }

    [HttpGet("usuario")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetUserData()
    {
        try
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { Message = "Token inválido" });
            }

            var user = await _authRepository.GetUserAsync(userId);

            if (user == null)
            {
                return NotFound(new { Message = "Usuário não encontrado" });
            }

            _logger.LogInformation($"Dados do viewModel: {JsonSerializer.Serialize(user)}");

            var viewModel = new UserViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Cpf = user.Cpf,
                address = user.address
            };


            return Ok(viewModel);
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Falha ao carregar dados de usuário");
            throw;
        }

    }

    [HttpPost("register")]
    public async Task<ActionResult<UserViewModel>> CreateUser([FromBody] CreateUserDto dto)
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

            var existingUser = await _authRepository.FindUserByEmailAsync(dto.Email);

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

            await _authRepository.CreateUserAsync(user);

            var viewModel = new UserViewModel
            {
                Name = user.Name,
                Email = user.Email,
                Cpf = user.Cpf,
            };

            _logger.LogInformation($"Cliente criado com ID: {user.Id}");

            return StatusCode(201, new
            {
                success = true,
                message = "Usuário cadastrado com sucesso",
                data = viewModel,
            });
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Falha ao criar usuário");
            throw;
        }

    }

    [HttpPost("endereco/{id}")]
    public async Task<ActionResult<AddressViewModel>> CreateAddressAndBindUser([FromBody] CreateAddressDto dto, int id)
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

            var address = new Address
            {
                Cep = dto.Cep,
                Logradouro = dto.Logradouro,
                Numero = dto.Numero,
                Cidade = dto.Cidade,
                Estado = dto.Estado,
                Pais = dto.Pais,
            };

            await _authRepository.CreateAddressAndBindUser(address, id);

            var viewModel = new AddressViewModel
            {
                Id = address.Id,
                Cep = address.Cep,
                Logradouro = address.Logradouro,
                Numero = address.Numero,
                Cidade = address.Cidade,
                Estado = address.Estado,
                Pais = address.Pais,
            };
            return StatusCode(201, new
            {
                success = true,
                message = "Endereço criado e vinculado com sucesso",
                data = viewModel,
            });
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Falha ao cadastrar endereço");
            throw;
        }
    }
}