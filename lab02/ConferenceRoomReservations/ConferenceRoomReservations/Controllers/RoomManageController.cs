using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Admin")] 
public class RoomController : Controller
{
    private readonly IConferenceRepository _repo;

    public RoomController(IConferenceRepository repo)
    {
        _repo = repo;
    }

    [HttpGet("Room/Manage")]
    public IActionResult RoomManage()
    {
        var rooms = _repo.GetRooms();
        return View(rooms); // Pass list of rooms to the view
    }

    [HttpPost("Room/Delete")]
    public IActionResult Delete(int id)
    {
        // Assuming repository has a RemoveRoom method

        if (_repo.RemoveRoomById(id))
        {
            TempData["Success"] = $"Salka id: '{id}' została usunięta.";
        }
        else
        {
            TempData["Error"] = $"Nie znaleziono salki o id: '{id}'";
        }

        return RedirectToAction("Manage", "Room");
    }

    [HttpPost("Room/Add")]
    public IActionResult Add(string name, int capacity)
    {
        var newRoom = new Room
        {
            Name = name,
            Capacity = capacity
        };
        if (_repo.AddRoom(newRoom))
        {
            TempData["Success"] = $"Dodano nową salę: '{name}' o pojemności: {capacity}.";
        }
        else
        {
            TempData["Error"] = "Nie udało się dodać nowej sali.";
        }
        return RedirectToAction("Manage", "Room");
    }

}