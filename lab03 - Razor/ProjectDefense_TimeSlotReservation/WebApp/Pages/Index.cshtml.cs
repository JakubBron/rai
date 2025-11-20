using DataModelsLib.CustomTypes;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.ModelsInternal;

namespace WebApp.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly UserManager<User> _userManager;

    public IndexModel(ILogger<IndexModel> logger, UserManager<User> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    public string Username { get; set; }
    public string Role { get; set; }
    public async Task OnGetAsync()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                Username = user.UserName;
                Role = user.Role.ToString(); // enum -> string
            }
        }
    }
}
