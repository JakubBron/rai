using DataModelsLib.CustomTypes;
using DataModelsLib.Models;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;

namespace WebApp.Services
{
    public class SlotsGenerator
    {
        private readonly ApplicationDbContext _context;
        private TeacherAvailability _teacherAvailability;

        public SlotsGenerator(ApplicationDbContext context, TeacherAvailability teacherAvailability)
        {
            _context = context;
            _teacherAvailability = teacherAvailability;
        }

        public async Task Generate()
        {
            var firstDay = _teacherAvailability.FirstDay;
            var lastDay = _teacherAvailability.LastDay;
            var end = _teacherAvailability.EndTime;
            var duration = _teacherAvailability.DurationMins;


            for (var day = firstDay; day <= lastDay; day = day.AddDays(1))
            {
                var time = _teacherAvailability.StartTime;
                while (time + duration <= end)
                {
                    var startDateTime = day.Date + time;
                    var endDateTime = day.Date + time + duration;

                    var slot = new ReservationsModel()
                    {
                        StarTime = startDateTime,
                        EndTime = endDateTime,
                        TeacherAvailabilityId = _teacherAvailability.Id,
                        Status = Status.Free,
                        StudentId = null
                    };
                    _context.Reservations.Add(slot);
                    time += duration;
                }
            }

            await _context.SaveChangesAsync();
        }

    }
}
