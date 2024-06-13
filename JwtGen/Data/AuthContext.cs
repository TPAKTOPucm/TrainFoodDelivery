using JwtGen.Models;
using Microsoft.EntityFrameworkCore;

namespace JwtGen.Data
{
    public class AuthContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public AuthContext(DbContextOptions options) : base(options) { }
    }
}
