using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataModelsLib.Models;
using WebApp.Data;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Pages.Rooms
{
    public class RoomEditModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Room Name")]
        public string RoomName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Room Number")]
        public string RoomNumber { get; set; } = string.Empty;
    }

    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public RoomEditModel Input { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == id);
            if (room == null)
            {
                return NotFound();
            }

            Input = new RoomEditModel
            {
                Id = room.Id,
                RoomName = room.RoomName,
                RoomNumber = room.RoomNumber
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == Input.Id);
            if (room == null)
            {
                return NotFound();
            }

            room.RoomName = Input.RoomName;
            room.RoomNumber = Input.RoomNumber;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Rooms.Any(r => r.Id == Input.Id))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToPage("./Index");
        }
    }
}
