using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataModelsLib.Models;
using Microsoft.AspNetCore.Identity;
using WebApp.Data;
using WebApp.ModelsInternal;

namespace WebApp.Pages.Rooms
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private UserManager<User> _userManager;

        public DeleteModel(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Room Room { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms.FirstOrDefaultAsync(m => m.Id == id);

            if (room == null)
            {
                return NotFound();
            }
            else
            {
                Room = room;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomToDelete = await _context.Rooms
                .Include(r => r.Availabilities)
                .ThenInclude(a => a.Reservations)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (roomToDelete == null)
            {
                return NotFound();
            }

            foreach (var availability in roomToDelete.Availabilities)
            {
                foreach (var reservation in availability.Reservations)
                {
                    if (!string.IsNullOrEmpty(reservation.StudentId))
                    {
                        var student = await _userManager.FindByIdAsync(reservation.StudentId);
                        if (student != null)
                        {
                            student.IsHoldingReservation = false;
                        }
                    }
                    _context.Reservations.Remove(reservation);
                }

                _context.TeacherAvailabilities.Remove(availability);
            }

            _context.Rooms.Remove(roomToDelete);

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
