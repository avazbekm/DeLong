using DeLong.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using DeLong.Service.Interfaces;
using DeLong.Domain.Configurations;
using DeLong.Application.DTOs.Customers;
using Microsoft.AspNetCore.Authorization;

namespace DeLong.WebAPI.Controllers;

[Authorize] // Faqat autentifikatsiya qilinganlar uchun
public class CustomerController : BaseController
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> AddAsync(CustomerCreationDto dto)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _customerService.AddAsync(dto)
        });

    [HttpPut("update")]
    public async Task<IActionResult> UpdateAsync(CustomerUpdateDto dto)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _customerService.ModifyAsync(dto)
        });

    [HttpDelete("remove/{id:long}")]
    public async Task<IActionResult> DestroyAsync(long id)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _customerService.RemoveAsync(id)
        });

    [HttpGet("get/{id:long}")]
    public async Task<IActionResult> GetByIdAsync(long id)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _customerService.RetrieveByIdAsync(id)
        });

    [HttpGet("get/INN/{INN:int}")]
    public async Task<IActionResult> GetByInnAsync(int INN)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _customerService.RetrieveByInnAsync(INN)
        });

 
    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllAsync([FromQuery] PaginationParams @params, [FromQuery] Filter filter, [FromQuery] string search = null)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _customerService.RetrieveAllAsync(@params, filter, search)
        });

    [HttpGet("get-all-customers")]
    public async Task<IActionResult> GetAllCustomersAsync()
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _customerService.RetrieveAllAsync()
        });
}