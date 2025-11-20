using DataModelsLib.CustomTypes;
using Microsoft.AspNetCore.Identity;

namespace WebApp.ModelsInternal
{
    public class User: IdentityUser
    {
        public Role Role { get; set; }
        public bool IsHoldingReservation { get; set; }
    }
}
