namespace JwtGen.DTOs;

public class LoginResponseDto
{
    public string AccessToken { get; set; }
    public DateTime AccessExpires { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshExpires { get; set; }
}
