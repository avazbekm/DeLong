using DeLong.Service.DTOs.Debts;
using DeLong.Service.Interfaces;
using DeLong.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeLong.WebAPI.Controllers;

[Authorize] // Faqat autentifikatsiya qilinganlar uchun
public class DebtController : BaseController
{
    private readonly IDebtService _debtService;

    public DebtController(IDebtService debtService)
    {
        _debtService = debtService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> AddAsync(DebtCreationDto dto)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _debtService.AddAsync(dto)
        });

    [HttpGet("get/{id:long}")]
    public async Task<IActionResult> GetByIdAsync(long id)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _debtService.RetrieveByIdAsync(id)
        });

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllAsync()
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _debtService.RetrieveAllAsync()
        });

    [HttpGet("get-by-sale/{saleId:long}")]
    public async Task<IActionResult> GetBySaleIdAsync(long saleId)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _debtService.RetrieveBySaleIdAsync(saleId)
        });

    [HttpGet("get-all-grouped-by-customer")]
    public async Task<IActionResult> GetAllGroupedByCustomerAsync()
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _debtService.RetrieveAllGroupedByCustomerAsync()
        });
}