using DataModelsLib.Models;
using DataModelsLib.CustomTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.ModelsInternal;
using WebApp.Data.Migrations;

namespace WebApp.Pages.Reservation
{
    [Authorize(Roles = RoleNames.Prowadz¹cy)]
    public class MoveModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public MoveModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public int TargetSlotId { get; set; }

        public ReservationsModel CurrentReservation { get; set; } = default!;
        public string UserEmail { get; set; } = "unknown";
        public List<ReservationsModel> AvailableSlots { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            // Validation in script in on purpose.
            if (id == null)
            {
                return NotFound();
            }

            CurrentReservation = await _context.Reservations.FirstOrDefaultAsync(r => r.Id == id);

            if (CurrentReservation == null)
            {
                return NotFound();
            }
            if (CurrentReservation.Status == Status.Blocked && CurrentReservation.StudentId == null)
            {
                return RedirectToPage("./ActionNotAllowed", new { reason = ActionNotAllowedReasons.SlotBlocked });
            }
            if (CurrentReservation.StudentId != null)
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Id == CurrentReservation.StudentId);
                UserEmail = user?.Email ?? "unknown";
            }

            var teacherAvailabilityId = CurrentReservation.TeacherAvailabilityId;

            AvailableSlots = await _context.Reservations
                .Where(r =>
                    r.Id != CurrentReservation.Id &&
                    r.StudentId == null &&
                    r.Status == Status.Free &&
                    r.StarTime > DateTime.Now)
                .OrderBy(r => r.StarTime)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var currentReservation = await _context.Reservations
                .FirstOrDefaultAsync(r => r.Id == id);

            if (currentReservation == null)
            {
                return NotFound();
            }
            if (currentReservation.Status == Status.Blocked && currentReservation.StudentId == null)
            {
                return RedirectToPage("./ActionNotAllowed", new { reason = ActionNotAllowedReasons.SlotBlocked });
            }

            var targetReservation = await _context.Reservations
                .FirstOrDefaultAsync(r => r.Id == TargetSlotId);

            if (targetReservation == null)
            {
                return NotFound();
            }

            // Move student
            targetReservation.StudentId = currentReservation.StudentId;
            targetReservation.Status = Status.Blocked;

            currentReservation.StudentId = null;
            currentReservation.Status = Status.Free;

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
