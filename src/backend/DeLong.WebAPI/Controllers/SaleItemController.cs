using DeLong.Domain.Configurations;
using DeLong.Service.DTOs.SaleItems;
using DeLong.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DeLong.WebAPI.Controllers;

public class SaleItemController : BaseController
{
    private readonly ISaleItemService saleItemService;

    public SaleItemController(ISaleItemService saleItemService)
    {
        this.saleItemService = saleItemService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> AddAsync(SaleItemCreationDto dto)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await this.saleItemService.AddAsync(dto)
        });

    [HttpPut("update")]
    public async Task<IActionResult> UpdateAsync(SaleItemUpdateDto dto)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await this.saleItemService.ModifyAsync(dto)
        });

    [HttpDelete("remove/{id:long}")]
    public async Task<IActionResult> DestroyAsync(long id)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await this.saleItemService.RemoveAsync(id)
        });

    [HttpGet("get/{id:long}")]
    public async Task<IActionResult> GetByIdAsync(long id)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await this.saleItemService.RetrieveByIdAsync(id)
        });

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllAsync([FromQuery] PaginationParams @params, Filter filter)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await this.saleItemService.RetrieveAllAsync(@params, filter)
        });

    [HttpGet("get-by-sale/{saleId:long}")]
    public async Task<IActionResult> GetBySaleIdAsync(long saleId)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await this.saleItemService.RetrieveAllBySaleIdAsync(saleId)
        });
}