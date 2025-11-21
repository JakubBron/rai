using DataModelsLib.CustomTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using SQLitePCL;
using WebApp.Data;
using WebApp.ModelsInternal;
using WebApp.Services;

namespace WebApp.Pages.Reservation
{
    public class InputModel: IValidatableObject
    {
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Start { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime End { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Start >= End)
            {
                yield return new ValidationResult(ValidationResultsMessages.StartTimeAfterEndTime, new[] { nameof(Start), nameof(End) });
            }
        }
    }

    [Authorize(Roles = RoleNames.Prowadzący)]
    public class BlockSlotsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private UserManager<User> _userManager;

        public BlockSlotsModel(ApplicationDbContext context, UserManager<User>userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();


        public void OnGet()
        {
            DateTime now = DateTime.Now;
            Input.Start = DateTime.Now.AddSeconds(-now.Second).AddMilliseconds(-now.Millisecond);
            Input.End = DateTime.Now.AddHours(1).AddSeconds(-now.Second).AddMilliseconds(-now.Millisecond);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            SlotsBlocker sb = new SlotsBlocker(_context, _userManager);
            await sb.Block(Input.Start, Input.End);

            return RedirectToPage("./Index");
        }

    }
}