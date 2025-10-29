using System.Collections.Generic;

public interface IConferenceRepository
{
    IEnumerable<Room> GetRooms();
    bool AddRoom(Room room);

    bool RemoveRoom(Room room);

    IEnumerable<Reservation> GetReservations();
    bool TryMakeReservation(Reservation reservation); // Thread-safe

    bool RemoveRoomById(int id);
}
  