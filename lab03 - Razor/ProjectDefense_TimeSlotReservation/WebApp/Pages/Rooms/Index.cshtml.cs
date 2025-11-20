using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataModelsLib.Models;
using WebApp.Data;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Pages.Rooms
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public IList<Room> Rooms { get; set; } = default!;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task OnGetAsync()
        {
            Rooms = await _context.Rooms.ToListAsync();
        }
    }
}
