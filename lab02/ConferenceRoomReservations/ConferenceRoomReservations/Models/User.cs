using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using ConferenceRoomReservations.Helpers;

public class User
{
    [Required]
    public string Username { get; set; }

    [Required, DataType(DataType.Password)]
    public string Password { get; set; }

    private string _role = "User";
    public string Role
    {
        get => _role;
        set => _role = string.IsNullOrWhiteSpace(value) ? AppRoles.User : value;
    }
}

