using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModelsLib.DTOs
{
    // ONLY free slots!
    public class SlotDTO
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Timespan { get; set; }
        public string TeacherEmail { get; set; }
        public RoomDTO Room { get; set; }

    }
}