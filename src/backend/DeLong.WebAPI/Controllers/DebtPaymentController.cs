using DeLong.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using DeLong.Service.Interfaces;
using DeLong.Service.DTOs.DebtPayments;

namespace DeLong.WebAPI.Controllers;

public class DebtPaymentController : BaseController
{
    private readonly IDebtPaymentService debtPaymentService;

    public DebtPaymentController(IDebtPaymentService debtPaymentService)
    {
        this.debtPaymentService = debtPaymentService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> AddAsync(DebtPaymentCreationDto dto)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await this.debtPaymentService.AddAsync(dto)
        });

    [HttpGet("get-by-debt/{debtId:long}")]
    public async Task<IActionResult> GetByDebtIdAsync(long debtId)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await this.debtPaymentService.RetrieveByDebtIdAsync(debtId)
        });

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllAsync()
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await this.debtPaymentService.RetrieveAllAsync()
        });
}