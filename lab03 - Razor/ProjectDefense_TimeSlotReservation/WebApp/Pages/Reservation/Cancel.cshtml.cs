using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataModelsLib.Models;
using WebApp.Data;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DataModelsLib.CustomTypes;
using Microsoft.AspNetCore.Identity;
using WebApp.Data.Migrations;
using WebApp.ModelsInternal;

namespace WebApp.Pages.Reservation
{
    public class CancelViewModel
    {
        public int Id { get; set; }

        [Display(Name = "From")]
        public DateTime StartTime { get; set; }
        [Display(Name = "To")]
        public DateTime EndTime { get; set; }
        [Display(Name = "Duration")]
        public string Timespan { get; set; }
        [Display(Name = "Who reserved?")]
        public string StudentEmail { get; set; } = "Free"; // show 'Free' if no student
        [Display(Name = "To who?")]
        public string TeacherEmail { get; set; } = default!;
        [Display(Name = "Where?")]
        public string RoomDisplay { get; set; } = default!;
    }

    public class CancelModel : PageModel
    {
        private readonly WebApp.Data.ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public CancelModel(WebApp.Data.ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public CancelViewModel CancelViewModel { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservations = await _context.Reservations.FirstOrDefaultAsync(m => m.Id == id);
            var currentlyLoggedIn = await _userManager.GetUserAsync(User);

            if (reservations == null)
            {
                return NotFound();
            }
            else if (reservations.Status == Status.Free)
            {
                return RedirectToPage("./ActionNotAllowed",
                    new { reason = ActionNotAllowedReasons.ReservationNotTaken });
            }
            else
            {
                // cant cancel someones visit if is Student. Prowadzący can do it.
                if (currentlyLoggedIn.Id != reservations.StudentId && currentlyLoggedIn.Role == Role.Student)
                {
                    return RedirectToPage("./ActionNotAllowed", new { reason = ActionNotAllowedReasons.NotOwner });
                }
                else
                {
                    var studentEmail = reservations.StudentId != null
                        ? (await _userManager.FindByIdAsync(reservations.StudentId))?.Email ?? "Unknown"
                        : "Free";
                    var teacherAvailability =
                        await _context.TeacherAvailabilities.FirstOrDefaultAsync(a =>
                            a.Id == reservations.TeacherAvailabilityId);
                    var teacherEmail = teacherAvailability != null
                        ? (await _userManager.FindByIdAsync(teacherAvailability.TeacherId))?.Email ?? "Unknown"
                        : "Unknown";

                    CancelViewModel = new CancelViewModel();
                    CancelViewModel.StartTime = reservations.StarTime;
                    CancelViewModel.EndTime = reservations.EndTime;
                    CancelViewModel.Timespan = (reservations.EndTime - reservations.StarTime).ToString(@"hh\:mm");
                    CancelViewModel.Id = reservations.Id;
                    CancelViewModel.StudentEmail = studentEmail;
                    CancelViewModel.TeacherEmail = teacherEmail;
                    CancelViewModel.RoomDisplay = await _context.Rooms
                        .Where(room => room.Id == teacherAvailability.RoomId)
                        .Select(room => $"{room.RoomNumber} ({room.RoomName})")
                        .FirstOrDefaultAsync() ?? "Unknown Room";
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ReservationsModel reservations = await _context.Reservations.FindAsync(id);
            User currentUser = await _userManager.GetUserAsync(User);
            if (reservations == null)
            {
                return NotFound();
            }
            else if (reservations.Status == Status.Free)
            {
                return RedirectToPage("./ActionNotAllowed", new { reason = ActionNotAllowedReasons.ReservationNotTaken });
            }
            else if (currentUser.Id != reservations.StudentId && currentUser.Role == Role.Student)
            {
                return RedirectToPage("./ActionNotAllowed", new { reason = ActionNotAllowedReasons.NotOwner });
            }
            else
            {
                ReservationsModel reservationToRemove = reservations;
                // if Prowadzący cancels, should set cancelled student's IsHoldingReservation to false
                if (currentUser.Role == Role.Prowadzący)
                {
                    var studentToCancel = await _userManager.FindByIdAsync(reservationToRemove.StudentId);
                    if (studentToCancel != null)
                    {
                        studentToCancel.IsHoldingReservation = false;
                    }
                }
                else
                {
                    currentUser.IsHoldingReservation = false;
                }
                reservationToRemove.StudentId = null;
                reservationToRemove.Status = Status.Free;

                _context.Reservations.Update(reservationToRemove);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
