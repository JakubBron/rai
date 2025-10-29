using System.Collections.Concurrent;
using ConferenceRoomReservations.Helpers;

public class UserRepository : IUserRepository
{
    // Thread-safe storage
    private readonly ConcurrentDictionary<string, User> _users = new();

    public UserRepository()
    {
        // Add default admin
        AddUser(new User { Username = "admin", Password = "admin123", Role = AppRoles.Admin });
        // Add default user
        AddUser(new User { Username = "user", Password = "user123", Role = AppRoles.User });
    }

    public IEnumerable<User> GetAllUsers() => _users.Values;

    public bool AddUser(User user)
    {
        return _users.TryAdd(user.Username, user);
    }

    public User? ValidateCredentials(string username, string password)
    {
        if (_users.TryGetValue(username, out var user))
        {
            if (user.Password == password)
                return user;
        }
        return null;
    }
}
