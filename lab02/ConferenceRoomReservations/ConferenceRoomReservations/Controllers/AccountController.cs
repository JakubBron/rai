using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using ConferenceRoomReservations.Helpers;

public class AccountController : Controller
{
    private readonly IUserRepository _userRepository;

    public AccountController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(User model, string? returnUrl = null)
    {
        if (!ModelState.IsValid)
            return View(model);

        // Use UserRepository to validate user
        var user = _userRepository.ValidateCredentials(model.Username, model.Password);
        if (user != null)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            // Redirect based on role
            if (user.Role == AppRoles.Admin)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Calendar", "Booking");
            }
        }

        ModelState.AddModelError("", "Invalid username or password");
        return View(model);
    }

    [HttpGet("Account/Login/{login}")]
    public async Task<IActionResult> LoginByUrl(string login)
    {
        // Find user by username
        var user = _userRepository.GetAllUsers()
            .FirstOrDefault(u => u.Username.Equals(login, StringComparison.OrdinalIgnoreCase));

        if (user == null)
            return NotFound($"User '{login}' not found.");

        // Create claims
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        // Sign in
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        // Redirect to calendar
        return RedirectToAction("Calendar", "Booking");
    }


    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Denied()
    {
        return View("AccessDenied");
    }
}
