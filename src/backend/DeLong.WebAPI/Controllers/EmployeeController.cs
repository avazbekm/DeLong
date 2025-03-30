using DeLong.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using DeLong.Service.Interfaces;
using DeLong.Service.DTOs.Employee;
using Microsoft.AspNetCore.Authorization;

namespace DeLong.WebAPI.Controllers;


[Authorize] // Faqat autentifikatsiya qilinganlar uchun
public class EmployeeController : BaseController
{
    private readonly IEmployeeService _employeeService;

    public EmployeeController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> AddAsync(EmployeeCreationDto dto)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _employeeService.AddAsync(dto)
        });

    [HttpPut("update")]
    public async Task<IActionResult> UpdateAsync(EmployeeUpdateDto dto)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _employeeService.ModifyAsync(dto)
        });

    [HttpDelete("remove/{id:long}")]
    public async Task<IActionResult> DestroyAsync(long id)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _employeeService.RemoveAsync(id)
        });

    [HttpGet("get/{id:long}")]
    public async Task<IActionResult> GetByIdAsync(long id)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _employeeService.RetrieveByIdAsync(id)
        });

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllAsync()
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _employeeService.RetrieveAllAsync()
        });

    [HttpGet("any")]
    public async Task<IActionResult> AnyAsync()
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _employeeService.AnyEmployeesAsync()
        });


}