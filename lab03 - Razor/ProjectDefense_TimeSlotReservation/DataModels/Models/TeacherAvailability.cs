using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace DataModelsLib.Models
{
    public class TeacherAvailability
    {
        [Key]
        public int Id { get; set; }
        public string TeacherId { get; set; } = default!;

        public int RoomId { get; set; }
        public Room Room { get; set; }
        public DateTime FirstDay { get; set; }
        public DateTime LastDay { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public TimeSpan DurationMins { get; set; }

        public ICollection<Reservations> Reservations { get; set; } 
    }
}
