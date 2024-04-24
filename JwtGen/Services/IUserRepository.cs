using JwtGen.Models;

namespace JwtGen.Services;

public interface IUserRepository
{
    public User GetUser(string id);
    public bool UpdateUser(User user);
    public bool AddUser(User user);
    public bool DeleteUser(User user);
}
