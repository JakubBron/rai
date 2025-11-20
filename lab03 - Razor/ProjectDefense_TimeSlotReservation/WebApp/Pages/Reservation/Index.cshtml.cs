using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using DataModelsLib.CustomTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataModelsLib.Models;
using Microsoft.AspNetCore.Identity;
using WebApp.Data;
using WebApp.ModelsInternal;
using System.ComponentModel.DataAnnotations;
using WebApp.Pages.Availability;

namespace WebApp.Pages.Reservation
{
    public class ReservationViewModel
    {
        public int Id { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        [DisplayName("Duration")]
        public string Timespan {get; set; }
        public string Status { get; set; } = default!;
        public string StudentEmail { get; set; } = "Free"; // show 'Free' if no student
        public string TeacherEmail { get; set; } = default!;
        public string RoomDisplay { get; set; } = default!;
    }

    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public IndexModel(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<ReservationViewModel> ReservationsView { get; set; } = default!;

        public async Task OnGetAsync()
        {
            var reservations = new List<ReservationsModel>();
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser.Role == Role.Student)
            {
                reservations = await _context.Reservations
                // reservations from now on , that are either: Free or taken by the current student
                .Where(r => r.StarTime > DateTime.Now && (r.Status == Status.Free || r.StudentId == currentUser.Id))
                .OrderBy(r => r.StarTime)
                .ToListAsync();
            }
            else
            {
                reservations = await _context.Reservations.Where(r => r.StarTime > DateTime.Now).OrderBy(r => r.StarTime).ToListAsync();
            }

            ReservationsView = new List<ReservationViewModel>();
            foreach (var r in reservations)
            {
                var studentEmail = r.StudentId != null
                    ? (await _userManager.FindByIdAsync(r.StudentId))?.Email ?? "Unknown"
                    : "Free";

                // Get TeacherAvailabilityId, find corresponding TeacherAvailability, then get TeacherId and find its email
                var teacherAvailability = await _context.TeacherAvailabilities
                    .FirstOrDefaultAsync(a => a.Id == r.TeacherAvailabilityId);

                var teacherEmail = teacherAvailability != null
                    ? (await _userManager.FindByIdAsync(teacherAvailability.TeacherId))?.Email ?? "Unknown" : "Unknown";

                var roomDisplay = await _context.Rooms.Where(room => room.Id == teacherAvailability.RoomId)
                    .Select(room => $"{room.RoomNumber} ({room.RoomName})")
                    .FirstOrDefaultAsync() ?? "Unknown Room";


                ReservationsView.Add(new ReservationViewModel
                {
                    Id = r.Id,
                    StartTime = r.StarTime,
                    EndTime = r.EndTime,
                    Timespan = $"{r.StarTime} - {r.EndTime}",
                    Status = r.Status.ToString(),
                    StudentEmail = studentEmail,
                    TeacherEmail = teacherEmail,
                    RoomDisplay = roomDisplay
                });
            }
        }

    }
}
