using DeLong.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private static Dictionary<string, string> refreshTokens = new Dictionary<string, string>(); // User -> Refresh Token

    public AuthController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginModel model)
    {
        if (model.Username == "admin" && model.Password == "1234")
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.Username)
            };

            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            // Refresh tokenni saqlaymiz (odatda DB yoki cache)
            refreshTokens[model.Username] = refreshToken;

            return Ok(new { accessToken, refreshToken });
        }
        return Unauthorized();
    }

    [HttpPost("refresh")]
    public IActionResult Refresh([FromBody] RefreshRequest request)
    {
        if (refreshTokens.TryGetValue(request.Username, out var savedRefreshToken) && savedRefreshToken == request.RefreshToken)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, request.Username)
            };

            var newAccessToken = _tokenService.GenerateAccessToken(claims);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            refreshTokens[request.Username] = newRefreshToken; // Eskisini almashtiramiz

            return Ok(new { accessToken = newAccessToken, refreshToken = newRefreshToken });
        }
        return Unauthorized(new { message = "Refresh token noto‘g‘ri yoki eskirgan!" });
    }

    [Authorize]
    [HttpGet("protected")]
    public IActionResult GetProtectedData()
    {
        return Ok(new { message = "Siz himoyalangan API-ga muvaffaqiyatli kirdingiz!" });
    }
}

public class LoginModel
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class RefreshRequest
{
    public string Username { get; set; }
    public string RefreshToken { get; set; }
}
