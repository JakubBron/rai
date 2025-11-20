using DataModelsLib.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.ModelsInternal;

namespace WebApp.Pages.Ban
{
    public class BanModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public BanModel(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<User> ActiveUsers { get; set; } = new();
        public List<User> BannedUsers { get; set; } = new();

        [BindProperty]
        public string SelectedActiveUserId { get; set; }

        [BindProperty]
        public string SelectedBannedUserId { get; set; }

        public async Task OnGetAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var bannedIds = await _context.Blacklist
                .Select(b => b.StudentId)
                .ToListAsync();

            BannedUsers = await _userManager.Users
                .Where(u => bannedIds.Contains(u.Id))
                .OrderBy(u => u.Email)
                .ToListAsync();

            ActiveUsers = await _userManager.Users
                .Where(u => !bannedIds.Contains(u.Id) && u.Id != currentUser.Id)
                .OrderBy(u => u.Email)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostBanAsync()
        {
            if (string.IsNullOrWhiteSpace(SelectedActiveUserId))
                return RedirectToPage();

            _context.Blacklist.Add(new BlacklistEntity
            {
                StudentId = SelectedActiveUserId
            });

            await _context.SaveChangesAsync();
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUnbanAsync()
        {
            if (string.IsNullOrWhiteSpace(SelectedBannedUserId))
                return RedirectToPage();

            var entry = await _context.Blacklist
                .FirstOrDefaultAsync(e => e.StudentId == SelectedBannedUserId);

            if (entry != null)
            {
                _context.Blacklist.Remove(entry);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }
    }
}
