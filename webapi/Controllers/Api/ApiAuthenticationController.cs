using Microsoft.AspNetCore.Mvc;
using WebApi.Repositories.Api;
using WebApi.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;

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

            return Ok(new { AccessToken = accessToken, RefreshToken = refreshToken });
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

            return Ok(new { AccessToken = newAccessToken, RefreshToken = newRefreshToken });
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

        return Ok(user);
    }
}