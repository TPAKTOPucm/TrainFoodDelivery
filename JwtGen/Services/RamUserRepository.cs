using JwtGen.Models;

namespace JwtGen.Services;

public class RamUserRepository : IUserRepository
{
    private static IDictionary<string, User> _users;
    static RamUserRepository()
    {
        _users = new Dictionary<string, User>();
        _users.Add(new("Customer", new()
        {
            Id = "Customer"
        }));
        _users.Add(new("Admin", new()
        {
            Id = "Admin"
        }));
        _users.Add(new("Cooker", new()
        {
            Id = "Cooker"
        }));
        _users.Add(new("Deliverer", new()
        {
            Id = "Deliverer"
        }));
    }
    public bool AddUser(User user)
    {
        _users.Add(user.Id, user);
        return true;
    }

    public bool DeleteUser(User user)
    {
        return _users.Remove(user.Id);
    }

    public User GetUser(string id)
    {
        try
        {
            return _users[id];
        } catch (Exception)
        {
            return null;
        }
    }

    public bool UpdateUser(User user)
    {
        _users[user.Id] = user;
        return true;
    }
}
