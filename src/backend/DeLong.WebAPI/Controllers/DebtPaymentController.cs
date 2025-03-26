using DeLong.Service.DTOs.DebtPayments;
using DeLong.Service.Interfaces;
using DeLong.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeLong.WebAPI.Controllers;

[Authorize] // Faqat autentifikatsiya qilinganlar uchun
public class DebtPaymentController : BaseController
{
    private readonly IDebtPaymentService _debtPaymentService;

    public DebtPaymentController(IDebtPaymentService debtPaymentService)
    {
        _debtPaymentService = debtPaymentService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> AddAsync(DebtPaymentCreationDto dto)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _debtPaymentService.AddAsync(dto)
        });

    [HttpGet("get-by-debt/{debtId:long}")]
    public async Task<IActionResult> GetByDebtIdAsync(long debtId)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _debtPaymentService.RetrieveByDebtIdAsync(debtId)
        });

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllAsync()
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _debtPaymentService.RetrieveAllAsync()
        });
}