using System.Collections.Concurrent;

public class ConferenceRepository : IConferenceRepository
{
    private readonly ConcurrentDictionary<string, Room> _rooms = new();
    private readonly List<Reservation> _reservations = new();
    private int _nextRoomId = 0;

    private readonly object _reservationLock = new();

    public IEnumerable<Room> GetRooms() => _rooms.Values;

    public Room GetRoomById(int roomId) => _rooms.Values.FirstOrDefault(r => r.Id == roomId);

    public bool AddRoom(Room room)
    {
        if (string.IsNullOrWhiteSpace(room.Name))
            return false;

        if (_rooms.ContainsKey(room.Name))
            return false;

        room.Id = Interlocked.Increment(ref _nextRoomId) - 1;
        return _rooms.TryAdd(room.Name, room);
    }

    public IEnumerable<Reservation> GetReservations() => _reservations;

    public bool TryMakeReservation(Reservation reservation)
    {
        lock (_reservationLock)
        {
            bool conflict = _reservations.Any(r =>
                r.RoomId == reservation.RoomId &&
                r.StartTime < reservation.EndTime &&
                reservation.StartTime < r.EndTime
            );

            if (conflict)
                return false;

            _reservations.Add(reservation);
            return true;
        }
    }

    public bool RemoveRoom(Room room)
    {
        if (room == null)
        {
            return false;
        }
        return _rooms.TryRemove(room.Name, out _);
    }

    public bool RemoveRoomById(int id)
    {
        var room = _rooms.Values.FirstOrDefault(r => r.Id == id);
        if (room == null)
            return false;
        return _rooms.TryRemove(room.Name, out _);
    }
}
