using DeLong.Application.Exceptions;
using DeLong.Service.Interfaces;
using DeLong.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DeLong.WebAPI.Controllers;

public class AuthController : BaseController
{
    private readonly ITokenService _tokenService;
    private readonly IEmployeeService _employeeService;
    private readonly IUserService _userService;

    public AuthController(ITokenService tokenService, IEmployeeService employeeService, IUserService userService)
    {
        _tokenService = tokenService;
        _employeeService = employeeService;
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginModel model)
    {
        try
        {
            var employee = await _employeeService.VerifyEmployeeAsync(model.Username, model.Password);
            var userRole = await _userService.RetrieveByIdAsync(employee.UserId);
            var claims = new List<Claim>
            {
                new Claim("UserId", employee.UserId.ToString()),
                new Claim("Username", employee.Username),
                new Claim("BranchId", employee.BranchId.ToString()),
                new Claim("Role", userRole.Role.ToString())
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