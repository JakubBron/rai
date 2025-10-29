using ConferenceRoomReservations.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceRoomReservations.Controllers
{

    [Authorize(Roles = AppRoles.Admin)]
    public class InitController : Controller
    {
        private readonly IConferenceRepository _conferenceRepo;
        private readonly IUserRepository _userRepo;

        public InitController(IConferenceRepository conferenceRepo, IUserRepository userRepo)
        {
            _conferenceRepo = conferenceRepo;
            _userRepo = userRepo;
        }

        [HttpGet]
        [Route("Init")]
        public IActionResult Init()
        {
            // 1️⃣ Add default rooms
            _conferenceRepo.AddRoom(new Room { Name = "Zerowa", Capacity = 1, Id = 0 });
            _conferenceRepo.AddRoom(new Room { Name = "Kreatywna", Capacity = 10, Id = 1 });
            _conferenceRepo.AddRoom(new Room { Name = "Innowacyjna", Capacity = 20, Id = 2 });
            _conferenceRepo.AddRoom(new Room { Name = "Standardowa", Capacity = 15, Id = 3 });

            // 2️⃣ Add default users (if they don’t exist)
            _userRepo.AddUser(new User { Username = "user2", Password = "user123", Role = "User" });
            _userRepo.AddUser(new User { Username = "user3", Password = "user123", Role = "User" });

            return View();
        }
    }
}
