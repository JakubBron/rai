using DataModelsLib.CustomTypes;
using DataModelsLib.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.ModelsInternal;

namespace WebApp.API.Slots
{
    public static class Booking
    {
        [AllowAnonymous]
        public static void MapBookSlotsEndpoint(this WebApplication app)
        {
            app.MapPost("/api/slots/{id}/book", async (int id, BookingRequest request, ApplicationDbContext db) =>
                {
                    var reservation = await db.Reservations.FindAsync(id);
                    if (reservation == null)
                    {
                        return Results.NotFound(ValidationResultsMessages.SlotNotFound);
                    }

                    if (reservation.Status != Status.Free)
                    {
                        return Results.BadRequest(ValidationResultsMessages.SlotTaken);
                    }

                    var student = await db.Users.FirstOrDefaultAsync(u => u.Email == request.StudentEmail);
                    if (student == null)
                    {
                        return Results.BadRequest(ValidationResultsMessages.UserNotFound);
                    }
                    if (student.Role != Role.Student)
                    {
                        return Results.BadRequest(ValidationResultsMessages.UserIsNotStudent);
                    }

                    if (student.IsHoldingReservation)
                    {
                        return Results.BadRequest(ValidationResultsMessages.UserHasReservations);
                    }

                    reservation.StudentId = student.Id;
                    reservation.Status = Status.Blocked;
                    student.IsHoldingReservation = true;

                    db.Reservations.Update(reservation);
                    await db.SaveChangesAsync();

                    return Results.Ok(new { Message = "Reservation sent!" });
                }).WithTags("Slots");
        }
    }
}