using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataModelsLib.CustomTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataModelsLib.Models;
using Microsoft.AspNetCore.Authorization;
using WebApp.Data;
using WebApp.Pages.Availability;
using Microsoft.AspNetCore.Identity;
using WebApp.ModelsInternal;
using WebApp.Data.Migrations;

namespace WebApp.Pages.Reservation
{
    public class OccupyDataModel
    {
        public int Id { get; set; }
    }

    [Authorize(Roles = RoleNames.Student)]
    public class OccupyModel : PageModel
    {
        private readonly WebApp.Data.ApplicationDbContext _context;
        private UserManager<User> _userManager;

        public OccupyModel(WebApp.Data.ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public ReservationsModel Reservations { get; set; } = default!;

        [BindProperty]
        public OccupyDataModel Input { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {   
                return NotFound();
            }
            var currentlyLoggedInStudent = await _userManager.GetUserAsync(User);
            if (currentlyLoggedInStudent.IsHoldingReservation)
            {
                return RedirectToPage("./ActionNotAllowed",
                    new { reason = ActionNotAllowedReasons.AlreadyHasReservation });
            }

            var reservations = await _context.Reservations.FirstOrDefaultAsync(m => m.Id == id);
            if (reservations == null)
            {
                return NotFound();
            }

            if (reservations.StarTime < DateTime.Now)
            {
                return RedirectToPage("./ActionNotAllowed", new { reason = ActionNotAllowedReasons.SlotMissed });
            }
            Reservations = reservations;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var currentlyLoggedInStudent = await _userManager.GetUserAsync(User);
                var reservationToOccupy = await _context.Reservations.FirstOrDefaultAsync(m => m.Id == Input.Id);
                if (reservationToOccupy == null)
                {
                    return NotFound();
                }

                if (currentlyLoggedInStudent == null)
                {
                    return NotFound();
                }

                if (currentlyLoggedInStudent.IsHoldingReservation)
                {
                    return RedirectToPage("./ActionNotAllowed",
                        new { reason = ActionNotAllowedReasons.AlreadyHasReservation });
                }

                if (reservationToOccupy.Status == Status.Blocked)
                {
                    return RedirectToPage("./ActionNotAllowed", new {reason = ActionNotAllowedReasons.AlreadyReserved});
                }
                if (reservationToOccupy.StarTime < DateTime.Now)
                {
                    return RedirectToPage("./ActionNotAllowed", new { reason = ActionNotAllowedReasons.SlotMissed });
                }

                reservationToOccupy.StudentId = currentlyLoggedInStudent.Id;
                reservationToOccupy.Status = Status.Blocked;
                currentlyLoggedInStudent.IsHoldingReservation = true;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservationsExists(Reservations.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ReservationsExists(int id)
        {
            return _context.Reservations.Any(e => e.Id == id);
        }
    }
}
