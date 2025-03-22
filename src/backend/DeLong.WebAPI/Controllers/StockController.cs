using DeLong.Application.DTOs.Stocks;
using DeLong.Domain.Configurations;
using DeLong.Service.Interfaces;
using DeLong.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DeLong.WebAPI.Controllers;

public class StockController : BaseController
{
    private readonly IStockService stockService;
    public StockController(IStockService stockService)
    {
        this.stockService = stockService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> AddAsync(StockCreationDto dto)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await this.stockService.AddAsync(dto)
        });

    [HttpPut("update")]
    public async Task<IActionResult> UpdateAsync(StockUpdateDto dto)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await this.stockService.ModifyAsync(dto)
        });

    [HttpDelete("delete/{id:long}")]
    public async Task<IActionResult> DeleteAsync(long id)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await this.stockService.RemoveAsync(id)
        });

    [HttpDelete("remove/{id:long}")]
    public async Task<IActionResult> DestroyAsync(long id)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await this.stockService.RemoveAsync(id)
        });

    [HttpGet("get/{id:long}")]
    public async Task<IActionResult> GetByIdAsync(long id)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await this.stockService.RetrieveByIdAsync(id)
        });

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllsync([FromQuery] PaginationParams @params, [FromQuery] Filter filter, string search)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await this.stockService.RetrieveAllAsync(@params, filter, search)
        });
}