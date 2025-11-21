using System.Net.Http.Json;
using DataModelsLib.DTOs;

const string BASE_URL = "https://localhost:7004";

HttpClient client = new HttpClient();
client.BaseAddress = new Uri(BASE_URL);

Console.WriteLine("=== Student Console Client ===");
Console.Write("Enter your student email: ");
string studentEmail = Console.ReadLine()!;

while (true)
{
    Console.WriteLine($"Current user: {studentEmail}");
    Console.WriteLine("\nChoose an option:");
    Console.WriteLine("1. Show available slots");
    Console.WriteLine("2. Book a slot");
    Console.WriteLine("3. Change user");
    Console.WriteLine("0. Exit");

    Console.Write("Your choice: ");
    string choice = Console.ReadLine()!;

    switch (choice)
    {
        case "1":
            await ShowAvailableSlots(client);
            break;

        case "2":
            await BookSlot(client, studentEmail);
            break;

        case "3":
            changeUser();
            break;

        case "0":
            return;

        default:
            Console.WriteLine("Invalid option. Try again.");
            break;
    }
}

async Task ShowAvailableSlots(HttpClient http)
{
    Console.WriteLine("\nFetching available slots...");
    List<SlotDTO> slots = new List<SlotDTO>();
    try
    {
        var result = await http.GetFromJsonAsync<List<SlotDTO>>("/api/slots/available");
        slots = result;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error fetching slots: {ex.Message}");
    }


    if (slots == null || slots.Count == 0)
    {
        Console.WriteLine("No free slots available.");
        return;
    }

    Console.WriteLine($"\nFound {slots.Count} free slots:\n");

    foreach (var s in slots)
    {
        Console.WriteLine($"[{s.Id}]  {s.StartTime} - {s.EndTime}");
        Console.WriteLine($"      Teacher: {s.TeacherEmail}");
        Console.WriteLine($"      Room:    {s.Room.RoomName} ({s.Room.RoomNumber})");
        Console.WriteLine($"      Length:  {s.Timespan} minutes\n");
    }
}

async Task BookSlot(HttpClient http, string studentEmail)
{
    Console.Write("\nEnter Slot ID to reserve: ");
    if (!int.TryParse(Console.ReadLine(), out int slotId))
    {
        Console.WriteLine("Invalid ID.");
        return;
    }

    var request = new BookingRequest
    {
        StudentEmail = studentEmail
    };

    var response = await http.PostAsJsonAsync($"/api/slots/{slotId}/book", request);

    if (response.IsSuccessStatusCode)
    {
        Console.WriteLine("Reservation completed!");
    }
    else
    {
        Console.WriteLine($"Error: {response.StatusCode}");
        Console.WriteLine(await response.Content.ReadAsStringAsync());
    }
}

void changeUser()
{
    Console.WriteLine("Type new username: ");
    studentEmail = Console.ReadLine()!;
}