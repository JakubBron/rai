using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.ModelsInternal;

namespace WebApp.Pages.TEMPADMIN
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;

        public IndexModel(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public string SelectedUserId { get; set; } = default!;

        [BindProperty]
        public bool NewIsHoldingReservation { get; set; }

        public List<User> Users { get; set; } = new();

        public async Task OnGetAsync()
        {
            Users = _userManager.Users.ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(SelectedUserId))
            {
                ModelState.AddModelError(string.Empty, "Please select a user.");
                Users = _userManager.Users.ToList();
                return Page();
            }

            var user = await _userManager.FindByIdAsync(SelectedUserId);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "User not found.");
                Users = _userManager.Users.ToList();
                return Page();
            }

            user.IsHoldingReservation = NewIsHoldingReservation;
            await _userManager.UpdateAsync(user);

            TempData["Success"] = $"User {user.Email}'s reservation flag updated successfully.";
            return RedirectToPage();
        }
    }
}