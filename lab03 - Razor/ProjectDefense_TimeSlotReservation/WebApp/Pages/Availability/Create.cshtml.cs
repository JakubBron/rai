using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DataModelsLib.Models;
using WebApp.Data;
using WebApp.Services;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace WebApp.Pages.Availability
{
    public class CreateTeacherAvailabilityModel
    {
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "First day")]
        public DateTime FirstDay { get; set; } = DateTime.Now.Date;
        
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Last day")]
        public DateTime LastDay { get; set; } = DateTime.Now.Date;

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Start Time")]
        public TimeSpan StartTime { get; set; } = TimeSpan.FromHours(8);

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "End Time")]
        public TimeSpan EndTime { get; set; } = TimeSpan.FromHours(12);

        [Required]
        [Range(1, 480, ErrorMessage = "Duration must be between 1 and 480 minutes")]
        [Display(Name = "Slot Duration (minutes)")]
        public int DurationMins { get; set; } = 15;

        [Required]
        [Display(Name = "Room")]
        public int RoomId { get; set; }
    }


    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public CreateTeacherAvailabilityModel Input { get; set; } = new();

        public List<Room> Rooms { get; set; } = default!;

        public void OnGet()
        {
            Rooms = _context.Rooms.ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Rooms = _context.Rooms.ToList();
                return Page();
            }

            var teacherId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (teacherId == null)
                return Unauthorized();


            bool overlaps = _context.TeacherAvailabilities.Any(a =>
                // same room
                (a.RoomId == Input.RoomId || a.TeacherId == teacherId) &&
                // dates intersection
                (Input.FirstDay <= a.LastDay && Input.LastDay >= a.FirstDay) &&
                // time intersection
                (Input.StartTime < a.EndTime && Input.EndTime > a.StartTime) &&
                // same room OR same teacher
                ((a.RoomId == Input.RoomId) || (a.TeacherId == teacherId))
            );

            if (overlaps)
            {
                ModelState.AddModelError(string.Empty, "You already have an availability that overlaps with these dates.");
                Rooms = _context.Rooms.ToList();
                return Page();
            }

            var availability = new TeacherAvailability
            {
                FirstDay = Input.FirstDay,
                LastDay = Input.LastDay,
                StartTime = Input.StartTime,
                EndTime = Input.EndTime,
                DurationMins = TimeSpan.FromMinutes(Input.DurationMins),
                RoomId = Input.RoomId,
                TeacherId = teacherId
            };

            _context.TeacherAvailabilities.Add(availability);
            await _context.SaveChangesAsync();

            var generator = new SlotsGenerator(_context, availability);
            await generator.Generate();

            return RedirectToPage("./Index");
        }

    }
}
