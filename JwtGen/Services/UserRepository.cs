using JwtGen.Data;
using JwtGen.Models;

namespace JwtGen.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthContext _db;
        public UserRepository(AuthContext db) 
        { 
            _db = db;
        }
        public bool AddUser(User user)
        {
            _db.Users.Add(user);
            _db.SaveChanges();
            return true;
        }

        public bool DeleteUser(User user)
        {
            _db.Users.Remove(user);
            _db.SaveChanges();
            return true;
        }

        public User GetUser(string id)
        {
            return _db.Users.Find(id);
        }

        public bool UpdateUser(User user)
        {
            _db.Update(user);
            _db.SaveChanges();
            return true;
        }
    }
}
