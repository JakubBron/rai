using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModelsLib.Models
{
    public class Room
    {
        [Key]
        public int Id { get; set; }
        public string RoomName { get; set; }
        public string RoomNumber { get; set; }

        public ICollection<TeacherAvailability> Availabilities { get; set; }
    }

}
