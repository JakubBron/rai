using DataModelsLib.CustomTypes;
using DataModelsLib.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.ModelsInternal;

namespace WebApp.Services
{
    public class SlotsBlocker
    {
        private readonly ApplicationDbContext _context;
        public UserManager<User> _userManager;

        public SlotsBlocker(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task Block(DateTime from, DateTime block)
        {
            var reservations = _context.Reservations.Where(r => r.StarTime >= from && r.EndTime <= block);
            foreach (var r in reservations)
            {
                if (r.Status == Status.Blocked) // someone already reserved or blocked this slot
                {
                    if (r.StudentId != null) // someone reserved it
                    {
                        var userWhoReserved = await _userManager.FindByIdAsync(r.StudentId);
                        if (userWhoReserved != null)
                        {
                            userWhoReserved.IsHoldingReservation = false;
                        }
                    }
                }
                r.Status = Status.Blocked;  // Block the slot
                r.StudentId = null;      // Remove any student association
            }
            await _context.SaveChangesAsync();
        }

    }
}