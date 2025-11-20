using DataModelsLib.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApp.ModelsInternal;

namespace WebApp.Data;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Room> Rooms { get; set; }
    public DbSet<TeacherAvailability> TeacherAvailabilities { get; set; }
    public DbSet<Reservations> Reservations { get; set; }
    public DbSet<BlacklistEntity> Blacklist { get; set; }
}
