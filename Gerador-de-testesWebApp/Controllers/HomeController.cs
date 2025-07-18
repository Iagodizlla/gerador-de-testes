using Microsoft.AspNetCore.Mvc;

namespace Gerador_de_testes.WebApp.Controllers;

public class HomeController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("erro")]
    public IActionResult Erro()
    {
        return View();
    }
}