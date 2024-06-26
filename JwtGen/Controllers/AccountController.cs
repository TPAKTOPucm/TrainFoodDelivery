using Gleeman.JwtGenerator;
using Gleeman.JwtGenerator.Generator;
using JwtGen.DTOs;
using JwtGen.Services;
using Microsoft.AspNetCore.Mvc;

namespace JwtGen.Controllers;
[ApiController]
[Route("[controller]/[action]")]
public class AccountController : ControllerBase
{

    private readonly ILogger<AccountController> _logger;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IUserRepository _repository;

    public AccountController(ILogger<AccountController> logger, ITokenGenerator tokenGenerator, IUserRepository repository)
    {
        _logger = logger;
        _tokenGenerator = tokenGenerator;
        _repository = repository;
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody]LoginDto dto)
    {
        var loginDto = new UserParameter()
        {
            Id = dto.Login
        };
        var user = _repository.GetUser(dto.Login);
        if (user == null || user.PasswordHash != dto.Password.GetHashCode())
            return NotFound();
        var token = await _tokenGenerator.GenerateAccessAndRefreshTokenAsync(loginDto, ExpireType.Hour);
        user.Token = token.AccessToken;
        user.TokenExpire = token.AccessExpire;
        _repository.UpdateUser(user);
        return Ok(new LoginResponseDto
        {
            RefreshToken = token.RefreshToken,
            RefreshExpires = token.RefreshExpire,
            AccessToken = token.AccessToken,
            AccessExpires = token.AccessExpire
        });
    }

    [HttpPost]
    public IActionResult Check([FromBody]string userId, string token)
    {
        if (_repository.GetUser(userId).Token == token)
            return Ok();
        return Unauthorized();
    }
}
