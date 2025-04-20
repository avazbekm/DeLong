using DeLong.WebAPI.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using DeLong.Service.Interfaces;
using DeLong.Application.Exceptions;

namespace DeLong.WebAPI.Controllers;

public class AuthController : BaseController
{
    private readonly ITokenService _tokenService;
    private readonly IUserService _userService;

    public AuthController(ITokenService tokenService, IEmployeeService employeeService, IUserService userService)
    {
        _tokenService = tokenService;
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginModel model)
    {
        try
        {
            var user = await _userService.VerifyUserAsync(model.Username, model.Password);
            var claims = new List<Claim>
            {
                new Claim("UserId", user.Id.ToString()),
                new Claim("Username", user.Username),
                new Claim("BranchId", user.BranchId.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString().ToLower())
            };

            var accessToken = _tokenService.GenerateAccessToken(claims);

            return Ok(new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = accessToken
            });
        }
        catch (NotFoundException ex)
        {
            return Unauthorized(new Response
            {
                StatusCode = 401,
                Message = ex.Message
            });
        }
        catch (Exception ex)
        {
            return Unauthorized(new Response
            {
                StatusCode = 401,
                Message = ex.Message
            });
        }
    }
}

public class LoginModel
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}