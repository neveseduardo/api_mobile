using Microsoft.AspNetCore.Mvc;
using WebApi.Models.Dto;
using WebApi.Repositories.Web;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace WebApi.Controllers.Web;

[Route("auth")]
public class WebAuthenticationController : Controller
{
    private readonly IWebAuthenticationRepository _repository;
    private readonly ILogger<WebAuthenticationController> _logger;

    public WebAuthenticationController(IWebAuthenticationRepository repository, ILogger<WebAuthenticationController> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    public IActionResult Index()
    {
        _logger.LogInformation("Página de login acessada.");
        return View();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromForm] WebLoginDto loginDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View("Index", loginDto);
            }


            var administrator = await _repository.ValidateAdministratorAsync(loginDto.email, loginDto.password);

            if (administrator == null)
            {
                ModelState.AddModelError(string.Empty, "Usuário ou senha inválidos.");
                return View("Index", loginDto);
            }

            var isAuthenticated = await _repository.SignInAsync(HttpContext, administrator);

            if (!isAuthenticated)
            {
                ModelState.AddModelError(string.Empty, "Erro ao criar sessão de login.");
                return View("Index", loginDto);
            }

            var returnUrl = Request.Form["ReturnUrl"].ToString();

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return Redirect("/");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro durante o processo de login para o email: {Email}", loginDto.email);
            ModelState.AddModelError(string.Empty, "Ocorreu um erro no servidor. Tente novamente mais tarde.");
            return View("Index", loginDto);
        }
    }

    [HttpGet("logout")]
    public async Task<IActionResult> Logout()
    {
        try
        {
            await _repository.SignOutAsync(HttpContext);

            return RedirectToAction("Index", "WebHome");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro durante o processo de logout.");
            ModelState.AddModelError(string.Empty, "Ocorreu um erro ao tentar sair. Tente novamente mais tarde.");
            return RedirectToAction("Index", "WebHome");
        }
    }
}