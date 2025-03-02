using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using webapi.Models;

namespace WebApi.Controllers;

public class ApiHomeController : Controller
{
    public ApiHomeController()
    {
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
