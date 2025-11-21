using DataModelsLib.CustomTypes;
using DataModelsLib.DTOs;
using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.ModelsInternal;


namespace WebApp.API.Slots
{
    public static class Available
    {

        public static void MapAvailableSlotsEndpoint(this WebApplication app)
        {
            List<SlotDTO> freeSlots = new List<SlotDTO>();
            app.MapGet("/api/slots/available", async (ApplicationDbContext db, UserManager<User> _userManager) =>
            {
                var reservations = await db.Reservations
                    .Where(r => r.StarTime > DateTime.Now && r.Status == Status.Free)
                .OrderBy(r => r.StarTime)
                .ToListAsync();
                
                foreach(var r in reservations)
                {
                    var teacherAvailability = db.TeacherAvailabilities.Where(t => t.Id == r.TeacherAvailabilityId).FirstOrDefault();
                    if(teacherAvailability == null)
                    {
                        continue;
                    }
                    
                    var room = await db.Rooms.FirstOrDefaultAsync(room => room.Id == teacherAvailability.Id);
                    var teacher = await _userManager.FindByIdAsync(teacherAvailability.TeacherId);
                    if (room != null)
                    {
                        freeSlots.Add(new SlotDTO
                        {
                            Id = r.Id,
                            StartTime = r.StarTime,
                            EndTime = r.EndTime,
                            Timespan = teacherAvailability.DurationMins.ToString(),
                            TeacherEmail = teacher.Email,
                            Room = new RoomDTO
                            {
                                Id = room.Id,
                                RoomName = room.RoomName,
                                RoomNumber = room.RoomNumber
                            }
                        });
                    }
                }
                
                return Results.Ok(freeSlots);
            }).WithTags("Rooms");
        }
    }
}
