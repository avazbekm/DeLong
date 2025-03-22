using DeLong.Service.DTOs.CashWarehouse;
using DeLong.Service.Interfaces;
using DeLong.WebAPI.Controllers;
using DeLong.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;

public class CashWarehouseController : BaseController
{
    private readonly ICashWarehouseService _service;

    public CashWarehouseController(ICashWarehouseService service)
    {
        _service = service;
    }

    [HttpPost("create")]
    public async Task<IActionResult> AddAsync(CashWarehouseCreationDto dto)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _service.AddAsync(dto)
        });

    [HttpPut("update")]
    public async Task<IActionResult> UpdateAsync(CashWarehouseUpdateDto dto)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _service.ModifyAsync(dto)
        });

    [HttpDelete("remove/{id:long}")]
    public async Task<IActionResult> DestroyAsync(long id)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _service.RemoveAsync(id)
        });

    [HttpGet("get")]
    public async Task<IActionResult> GetByIdAsync()
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _service.RetrieveByIdAsync()
        });

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllAsync()
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _service.RetrieveAllAsync()
        });
}