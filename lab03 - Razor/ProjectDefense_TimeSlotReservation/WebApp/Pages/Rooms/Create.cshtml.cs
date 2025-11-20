using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DataModelsLib.Models;
using WebApp.Data;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Pages.Rooms
{
    public class RoomCreateModel
    {
        [Required]
        [Display(Name = "Room Name")]
        public string RoomName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Room Number")]
        public string RoomNumber { get; set; } = string.Empty;
    }

    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public RoomCreateModel Input { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var room = new Room();
            room.RoomName = Input.RoomName;
            room.RoomNumber = Input.RoomNumber;

            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
