using Gleeman.JwtGenerator;
using Gleeman.JwtGenerator.Generator;
using JwtGen.DTOs;
using JwtGen.Services;
using Microsoft.AspNetCore.Mvc;

namespace JwtGen.Controllers;
[ApiController]
[Route("[controller]")]
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

    [HttpGet]
    public async Task<IActionResult> Login(UserParameter loginDto)
    {
        var user = _repository.GetUser(loginDto.Id);
        if (user == null)
            return NotFound();
        var token = await _tokenGenerator.GenerateAccessAndRefreshTokenAsync(loginDto, ExpireType.Hour);
        user.Token = token.RefreshToken;
        user.TokenExpire = token.RefreshExpire;
        _repository.UpdateUser(user);
        return Ok(new LoginResponseDto
        {
            RefreshToken = token.RefreshToken,
            RefreshExpires = token.RefreshExpire,
            AccessToken = token.AccessToken,
            AccessExpires = token.AccessExpire
        });
    }
}
