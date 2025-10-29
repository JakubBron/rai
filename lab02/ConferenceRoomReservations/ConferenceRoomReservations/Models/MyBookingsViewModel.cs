using System.Collections.Generic;

public class MyBookingsViewModel
{
    public List<Reservation> Bookings { get; set; }
    public Dictionary<int, string> RoomsById { get; set; }
}