using DataModelsLib.CustomTypes;
using System.ComponentModel.DataAnnotations;

namespace DataModelsLib.Models;

public class Reservations
{
    [Key]
    public int Id { get; set; }
    public int TeacherAvailabilityId { get; set; }
    public DateTime StarTime { get; set; }
    public DateTime EndTime { get; set; }

    public string? StudentId { get; set; }  // Student = User z Role = Student; User jest z IdentityUser, wiec klucz obcy to string (IdentityUser.Id)
    public Status status { get; set; }

}