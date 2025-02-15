using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using webapi.Models;

namespace webapi.Controllers.Api;

public class ApiHomeController : Controller
{
    private readonly ILogger<ApiHomeController> _logger;

    public ApiHomeController(ILogger<ApiHomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return Ok(new
        {
            Name = "APIV1",
            Version = "v1.0.0",
        });
    }
}
