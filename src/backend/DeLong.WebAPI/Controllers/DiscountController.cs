using DeLong.Service.Interfaces;
using DeLong.WebAPI.Models; // Response modeli uchun
using DeLong_Desktop.ApiService.DTOs.Discounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeLong.WebAPI.Controllers;

[Authorize] // Faqat autentifikatsiya qilinganlar uchun
public class DiscountController : BaseController
{
    private readonly IDiscountService _discountService;

    public DiscountController(IDiscountService discountService)
    {
        _discountService = discountService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> AddAsync(DiscountCreationDto dto)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _discountService.AddAsync(dto)
        });

    [HttpPut("update")]
    public async Task<IActionResult> UpdateAsync(DiscountUpdateDto dto)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _discountService.ModifyAsync(dto)
        });

    [HttpDelete("remove/{id:long}")]
    public async Task<IActionResult> DestroyAsync(long id)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _discountService.RemoveAsync(id)
        });

    [HttpGet("get/{id:long}")]
    public async Task<IActionResult> GetByIdAsync(long id)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _discountService.RetrieveByIdAsync(id)
        });

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllAsync()
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _discountService.RetrieveAllAsync()
        });

    [HttpGet("get-by-sale/{saleId:long}")]
    public async Task<IActionResult> GetBySaleIdAsync(long saleId)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _discountService.RetrieveBySaleIdAsync(saleId)
        });
}