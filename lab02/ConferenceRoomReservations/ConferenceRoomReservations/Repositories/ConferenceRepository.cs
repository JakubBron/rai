using System.Collections.Concurrent;

public class ConferenceRepository : IConferenceRepository
{
    private readonly ConcurrentDictionary<string, Room> _rooms = new();
    private readonly List<Reservation> _reservations = new();
    private int _nextRoomId = 0;

    private readonly object _reservationLock = new();

    public IEnumerable<Room> GetRooms() => _rooms.Values;

    public bool AddRoom(Room room)
    {
        if (string.IsNullOrWhiteSpace(room.Name))
            return false;

        if (_rooms.ContainsKey(room.Name))
            return false;

        room.Id = Interlocked.Increment(ref _nextRoomId) - 1;
        return _rooms.TryAdd(room.Name, room);
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


    public IEnumerable<Reservation> GetReservations()
    {
        lock (_reservationLock)
        {
            return _reservations.ToList();
        }
    }

    public bool TryMakeReservation(Reservation reservation)
    {
        if (!reservation.IsValid)
        {
            return false;
        }

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

    public Reservation FindReservationByParams(string username, DateTime startDate, int id)
    {
       var found = _reservations.Find(r =>
            r.RoomId == id &&
            r.StartTime == startDate &&
            r.UserName == username
        );

        return found;
    }

    public bool DeleteReservation(Reservation reservation)
    {
        lock (_reservationLock)
        {
            return _reservations.Remove(reservation);
        }
    }
}
