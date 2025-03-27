using DeLong.Service.DTOs.Prices;
using DeLong.Service.Interfaces;
using DeLong.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeLong.WebAPI.Controllers;

[Authorize] // Faqat autentifikatsiya qilinganlar uchun
public class PriceController : BaseController
{
    private readonly IPriceServer _priceService;

    public PriceController(IPriceServer priceService)
    {
        _priceService = priceService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> AddAsync(PriceCreationDto dto)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _priceService.AddAsync(dto)
        });

    [HttpPut("update")]
    public async Task<IActionResult> UpdateAsync(PriceUpdateDto dto)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _priceService.ModifyAsync(dto)
        });

    [HttpDelete("remove/{id:long}")]
    public async Task<IActionResult> DestroyAsync(long id)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _priceService.RemoveAsync(id)
        });

    [HttpGet("get/{id:long}")]
    public async Task<IActionResult> GetByIdAsync(long id)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _priceService.RetrieveByIdAsync(id)
        });

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllAsync()
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _priceService.RetrieveAllAsync()
        });

    [HttpGet("get-all-product/{productId:long}")]
    public async Task<IActionResult> GetAllByProductIdAsync(long productId)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _priceService.RetrieveAllAsync(productId)
        });
}