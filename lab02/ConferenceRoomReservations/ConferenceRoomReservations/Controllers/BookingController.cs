using ConferenceRoomReservations.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace ConferenceRoomReservations.Controllers
{
    [Authorize]
    public class BookingController : Controller
    {
        private readonly IConferenceRepository _repository;

        public BookingController(IConferenceRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("Booking/Calendar")]
        public IActionResult Calendar()
        {
            ViewBag.Date = DateTime.Now;
            return View();
        }

        [HttpGet]
        public IActionResult MyBookings()
        {
            var userName = User.Identity?.Name;
            if (string.IsNullOrEmpty(userName))
            {
                return Unauthorized(new { error = "User is not logged in." });
            }
            var today = DateTime.Today;
            var bookings = _repository.GetReservations()
                .Where(r => r.UserName == userName && r.StartTime.Date == today)
                .OrderBy(r => r.StartTime)
                .ToList();
            var rooms = _repository.GetRooms().ToDictionary(r => r.Id, r => r.Name);
            var model = new MyBookingsViewModel
            {
                Bookings = bookings,
                RoomsById = rooms
            };

            return View(model);
        }

        [HttpGet("Booking/GetForDay")]
        [Produces("application/json")]
        public IActionResult GetForDay()
        {
            var reservations = _repository.GetReservations()
                .Where(r => r.StartTime.Date == DateTime.Today)
                .Select(r => new
                {
                    r.UserName,
                    r.RoomId,
                    StartTime = r.StartTime.ToString("yyyy-MM-ddTHH:mm"),
                    EndTime = r.EndTime.ToString("yyyy-MM-ddTHH:mm")
                })
                .ToList();
            
            //var reservations = _repository.GetReservations();
            return Json(reservations);
        }

        [HttpPost("Booking/Create")]
        public IActionResult MakeReservation(string beginTime, int id)
        {
            var userName = User.Identity?.Name;
            if (string.IsNullOrEmpty(userName))
            {
                return Unauthorized(new { error = "User is not logged in." });
            }

            if (!DateTime.TryParseExact(beginTime, "HH:mm", null, System.Globalization.DateTimeStyles.None, out var startTime))
            {
                return BadRequest(new { error = "Invalid beginTime format. Expected HH:mm." });
            }

            if (id < 0)
            {
                return BadRequest(new { error = "Invalid room id!" });
            }

            var reservation = new Reservation
            {
                UserName = userName,
                RoomId = id,
                StartTime = startTime,
                EndTime = startTime.Add(AppReservationLimits.minReservationDuration),
            };

            if (!reservation.IsValid)
            {
                return BadRequest(new { error = "Invalid reservation details." });
            }

            var success = _repository.TryMakeReservation(reservation);
            if (!success)
            {
                return Conflict(new { error = "Could not make reservation. The room may already be booked for this time." });
            }

            return Ok(new { message = "Reservation successful!" });
        }


        [HttpDelete("Booking/Cancel")]
        public IActionResult DeleteReservation(string beginTime, int id)
        {
            var userName = User.Identity?.Name;
            if (string.IsNullOrEmpty(userName))
            {
                return Unauthorized(new { error = "User is not logged in." });
            }

            if (!DateTime.TryParseExact(beginTime, "HH:mm", null, System.Globalization.DateTimeStyles.None, out var startTime))
            {
                return BadRequest(new { error = "Invalid beginTime format. Expected HH:mm." });
            }

            if (id < 0)
            {
                return BadRequest(new { error = "Invalid room id!" });
            }

            var reservation = _repository.FindReservationByParams(userName, startTime, id);

            if (reservation == null)
            {
                return BadRequest(new { error = "Reservation does not exist." });
            }

            var success = _repository.DeleteReservation(reservation);
            if (!success)
            {
                return Conflict(new { error = "Could not delete reservation." });
            }

            return Ok(new { message = "Reservation deleted successfully!" });
        }


        [HttpGet("Booking/ExportMyBookings")]
        public IActionResult ExportMyBookings()
        {
            var userName = User.Identity?.Name;
            if (string.IsNullOrEmpty(userName))
                return Unauthorized("User is not logged in.");

            var today = DateTime.Today;
            var bookings = _repository.GetReservations()
                .Where(r => r.UserName == userName && r.StartTime.Date == today)
                .OrderBy(r => r.StartTime)
                .ToList();
            
            var roomsById = _repository.GetRooms().ToDictionary(r => r.Id, r => r.Name);

            var sb = new StringBuilder();
            string crlf = "\r\n"; // ICS requires CRLF

            sb.Append("BEGIN:VCALENDAR").Append(crlf);
            sb.Append("VERSION:2.0").Append(crlf);
            sb.Append("PRODID:-//MyConferenceApp//EN").Append(crlf);

            foreach (var r in bookings)
            {
                sb.Append("BEGIN:VEVENT").Append(crlf);
                sb.Append($"UID:{Guid.NewGuid()}").Append(crlf);
                sb.Append($"DTSTAMP:{DateTime.UtcNow:yyyyMMddTHHmmssZ}").Append(crlf);

                // Convert times to UTC and format correctly
                sb.Append($"DTSTART:{r.StartTime.ToUniversalTime():yyyyMMddTHHmmss}Z").Append(crlf);
                sb.Append($"DTEND:{r.EndTime.ToUniversalTime():yyyyMMddTHHmmss}Z").Append(crlf);

                // Escape special characters in SUMMARY/DESCRIPTION
                var roomName = roomsById.TryGetValue(r.RoomId, out var name) ? name : $"Room {r.RoomId}";
                var summary = $"Reservation in {roomName}".Replace(",", "\\,").Replace(";", "\\;");
                var description = $"Reserved by {r.UserName}".Replace(",", "\\,").Replace(";", "\\;");

                sb.Append($"SUMMARY:{summary}").Append(crlf);
                sb.Append($"DESCRIPTION:{description}").Append(crlf);
                sb.Append("STATUS:CONFIRMED").Append(crlf);
                sb.Append("END:VEVENT").Append(crlf);
            }

            sb.Append("END:VCALENDAR").Append(crlf);

            var bytes = Encoding.UTF8.GetBytes(sb.ToString());
            return File(bytes, "text/calendar", "MyBookings.ics");
        }


        [HttpGet("Booking/GetConferenceRooms")]
        [Produces("application/json")]
        public IActionResult GetConferenceRooms()
        {
            var rooms = _repository.GetRooms();
            return Json(rooms);
        }

        [HttpGet("Booking/GetConfig")]
        [Produces("application/json")]
        public IActionResult GetConfig()
        {
            var config = AppReservationLimits.minReservationDuration;
            return Json(config);
        }
    }
}
