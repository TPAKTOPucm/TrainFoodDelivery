namespace JwtGen.Models;

public class User
{
    public string Id { get; set; }
    public int PasswordHash { get; set; }
    public string? Token { get; set; }
    public DateTime? TokenExpire { get; set; }
}
