using DataModelsLib.DTOs;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;

namespace WebApp.API.Rooms
{
    public static class Rooms
    {
        public static void MapRoomsEndpoint(this WebApplication app)
        {
            app.MapGet("/api/rooms", async (ApplicationDbContext db) =>
            {
                var rooms = await db.Rooms
                    .Select(r => new RoomDTO
                    {
                        Id = r.Id,
                        RoomNumber = r.RoomNumber,
                        RoomName = r.RoomName
                    })
                    .ToListAsync();

                return Results.Ok(rooms);
            }).WithTags("Rooms");
        }
    }
}
