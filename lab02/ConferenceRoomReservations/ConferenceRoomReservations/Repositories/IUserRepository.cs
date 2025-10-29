using System.Collections.Generic;

public interface IUserRepository
{
    IEnumerable<User> GetAllUsers();
    bool AddUser(User user);
    User? ValidateCredentials(string username, string password); // returns User if valid
}
