using System;

public class Reservation
{
    public string UserName { get; set; }   // Who made the reservation
    public int RoomId { get; set; }   // Room being reserved
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}
