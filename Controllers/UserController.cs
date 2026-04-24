using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Proyecto.Data;

namespace Proyecto.Controllers;

public class UserController : Controller
{
    public IActionResult Books()
    {
        return View();
    }

    public IActionResult Users()
    {
        return View();
    }

    public IActionResult Loans()
    {
        return View();
    }
}