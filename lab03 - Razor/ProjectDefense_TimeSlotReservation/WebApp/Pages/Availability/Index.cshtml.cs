using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataModelsLib.Models;
using Microsoft.AspNetCore.Identity;
using WebApp.Data;
using WebApp.ModelsInternal;

namespace WebApp.Pages.Availability
{
    public class TeacherAvailabilityViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Whose reservation?")]
        public string TeacherEmail { get; set; }

        [Display(Name = "Dates from - to")]
        public string DaysDuration { get; set; }

        [Display(Name = "Hours from - to")]
        public string HoursDuration { get; set; }

        [Display(Name = "Time slot duration")]
        public TimeSpan DurationMins { get; set; }

        [Display(Name = "Room")]
        public string RoomDisplay { get; set; } = default!;
    }
    public class IndexModel : PageModel
    {
        private readonly WebApp.Data.ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public IndexModel(WebApp.Data.ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<TeacherAvailabilityViewModel> TeacherAvailability { get;set; } = default!;

        public async Task<string?> GetUserEmailAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return null;

            var user = await _userManager.FindByIdAsync(userId);
            return user?.Email;
        }

        public async Task OnGetAsync()
        {
            var availabilities = await _context.TeacherAvailabilities
                .Include(t => t.Room)
                .ToListAsync();

            TeacherAvailability = new List<TeacherAvailabilityViewModel>();

            foreach (var t in availabilities)
            {
                var email = await GetUserEmailAsync(t.TeacherId);

                TeacherAvailability.Add(new TeacherAvailabilityViewModel
                {
                    Id = t.Id,
                    TeacherEmail = email ?? "Unknown",
                    DaysDuration = $"{t.FirstDay} - {t.LastDay}",
                    HoursDuration = $"{t.StartTime} - {t.EndTime}",
                    DurationMins = t.DurationMins,
                    RoomDisplay = $"{t.Room.RoomNumber} ({t.Room.RoomName})"
                });
            }
        }
    }
}
