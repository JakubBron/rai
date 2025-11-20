using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModelsLib.Models
{
    public class BlacklistEntity
    {
        [Key]
        public int EntityId { get; set; }
        public string? StudentId { get; set; }  // Student = User z Rolą = Student, User jest z IdentityUser, więc klucz obcy to string (IdentityUser.Id)
    }
}
