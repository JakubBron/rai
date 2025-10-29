using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceRoomReservations.Controllers
{
    [Authorize]
    [Route("Booking/Calendar")]
    public class BookingController : Controller
    {
        [HttpGet]
        [Route("")]
        public IActionResult Calendar()
        {
            ViewData["Message"] = "Welcome to the Conference Room Booking Calendar!";
            return View();
        }
    }
}
