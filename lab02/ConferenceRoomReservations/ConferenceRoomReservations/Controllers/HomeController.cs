using System.Diagnostics;
using ConferenceRoomReservations.Helpers;
using Microsoft.AspNetCore.Mvc;
using ConferenceRoomReservations.Models;
using Microsoft.AspNetCore.Authorization;

namespace ConferenceRoomReservations.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index(string? authError)
    {

        if (authError == AppKeywords.AuthError_LoginRequired)
        {
            ViewBag.Error = AppKeywords.AuthError_LoginRequired;
        }

        var username = User.Identity?.IsAuthenticated == true ? User.Identity.Name : "niezalogowany";
        var role = User.IsInRole("Admin") ? "Admin" : (User.IsInRole("User") ? "User" : "niezalogowany");

        ViewBag.Username = username;
        ViewBag.Role = role;

        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
