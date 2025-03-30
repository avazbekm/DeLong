using DeLong.Application.DTOs.Users;
using DeLong.Domain.Configurations;
using DeLong.Service.Interfaces;
using DeLong.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeLong.WebAPI.Controllers;

[Authorize] // Faqat autentifikatsiya qilinganlar uchun
public class UserController : BaseController
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [AllowAnonymous]
    [HttpPost("create")]
    public async Task<IActionResult> AddAsync(UserCreationDto dto)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _userService.AddAsync(dto)
        });

    [HttpPut("update")]
    public async Task<IActionResult> UpdateAsync(UserUpdateDto dto)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _userService.ModifyAsync(dto)
        });

    [HttpDelete("delete/{id:long}")]
    public async Task<IActionResult> DeleteAsync(long id)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _userService.RemoveAsync(id)
        });

    [HttpDelete("remove/{id:long}")]
    public async Task<IActionResult> DestroyAsync(long id)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _userService.RemoveAsync(id)
        });

    [HttpGet("get/{id:long}")]
    public async Task<IActionResult> GetByIdAsync(long id)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _userService.RetrieveByIdAsync(id)
        });

    [HttpGet("get/jshshir")]
    public async Task<IActionResult> GetByJshshirAsync(string jshshir)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _userService.RetrieveByJSHSHIRAsync(jshshir)
        });

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllAsync([FromQuery] PaginationParams @params, [FromQuery] Filter filter, string search = null)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _userService.RetrieveAllAsync(@params, filter, search)
        });

    [HttpGet("get-allUsers")]
    public async Task<IActionResult> GetAllUsersAsync()
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _userService.RetrieveAllAsync()
        });
}