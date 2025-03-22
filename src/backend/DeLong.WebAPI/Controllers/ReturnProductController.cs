using DeLong.Domain.Configurations;
using DeLong.Service.DTOs;
using DeLong.Service.Interfaces;
using DeLong.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DeLong.WebAPI.Controllers;

public class ReturnProductController : BaseController
{
    private readonly IReturnProductService _returnProductService;

    public ReturnProductController(IReturnProductService returnProductService)
    {
        _returnProductService = returnProductService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> AddAsync([FromBody] ReturnProductCreationDto dto)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _returnProductService.AddAsync(dto)
        });

    [HttpPut("update")]
    public async Task<IActionResult> UpdateAsync([FromBody] ReturnProductUpdateDto dto)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _returnProductService.ModifyAsync(dto)
        });

    [HttpDelete("remove/{id:long}")]
    public async Task<IActionResult> RemoveAsync(long id)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _returnProductService.RemoveAsync(id)
        });

    [HttpGet("get/{id:long}")]
    public async Task<IActionResult> GetByIdAsync(long id)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _returnProductService.RetrieveByIdAsync(id)
        });

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllAsync([FromQuery] PaginationParams @params, Filter filter)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _returnProductService.RetrieveAllAsync()
        });

    [HttpGet("get-by-sale/{saleId:long}")]
    public async Task<IActionResult> GetBySaleIdAsync(long saleId)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _returnProductService.RetrieveBySaleIdAsync(saleId)
        });

    [HttpGet("get-by-product/{productId:long}")]
    public async Task<IActionResult> GetByProductIdAsync(long productId)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _returnProductService.RetrieveByProductIdAsync(productId)
        });
}