using DeLong.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using DeLong.Service.Interfaces;
using DeLong.Domain.Configurations;
using DeLong.Application.DTOs.Transactions;

namespace DeLong.WebAPI.Controllers;

public class TransactionController : BaseController
{
    private readonly ITransactionService transactionService;

    public TransactionController(ITransactionService transactionService)
    {
        this.transactionService = transactionService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> AddAsync([FromBody] TransactionCreationDto dto)
    {
        if (dto == null || dto.Items == null || !dto.Items.Any())
        {
            return Ok(new Response
            {
                StatusCode = 400,
                Message = "Transaction ma'lumotlari yoki mahsulotlar ro'yxati null bo'lmasligi kerak.",
                Data = null
            });
        }

        return Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await this.transactionService.AddAsync(dto)
        });
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateAsync([FromBody] TransactionUpdateDto dto)
    {
        if (dto == null)
        {
            return Ok(new Response
            {
                StatusCode = 400,
                Message = "Transaction ma'lumotlari null bo'lmasligi kerak.",
                Data = null
            });
        }

        return Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await this.transactionService.ModifyAsync(dto)
        });
    }

    [HttpDelete("remove/{id:long}")]
    public async Task<IActionResult> DestroyAsync(long id)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await this.transactionService.RemoveAsync(id)
        });

    [HttpGet("get/{id:long}")]
    public async Task<IActionResult> GetByIdAsync(long id)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await this.transactionService.RetrieveByIdAsync(id)
        });

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllAsync([FromQuery] PaginationParams @params, Filter filter)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await this.transactionService.RetrieveAllAsync()
        });
}