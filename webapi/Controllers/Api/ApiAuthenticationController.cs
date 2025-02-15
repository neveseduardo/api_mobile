using Microsoft.AspNetCore.Mvc;
using WebApi.Repositories;
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
    public async Task<IActionResult> AuthenticateCustomer([FromBody] LoginDto dto)
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

            var customer = await _authRepository.ValidateCustomerAsync(dto.Email, dto.Password);

            if (customer == null)
            {
                return Unauthorized();
            }

            var accessToken = _authRepository.CreateToken(customer);
            var refreshToken = _authRepository.CreateRefreshToken(customer);

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

            var customerIdClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (customerIdClaim == null || !int.TryParse(customerIdClaim, out int customerId))
            {
                return Unauthorized("Token inválido");
            }

            var customer = await _authRepository.GetCustomerAsync(customerId);

            if (customer == null)
            {
                return NotFound("Usuário não encontrado");
            }

            var newAccessToken = _authRepository.CreateToken(customer);
            var newRefreshToken = _authRepository.CreateRefreshToken(customer);

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

    [HttpGet("customer")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetCustomerData()
    {
        var customerIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (customerIdClaim == null || !int.TryParse(customerIdClaim, out int customerId))
        {
            return Unauthorized(new { Message = "Token inválido" });
        }

        var customer = await _authRepository.GetCustomerAsync(customerId);

        if (customer == null)
        {
            return NotFound(new { Message = "Usuário não encontrado" });
        }

        return Ok(customer);
    }
}