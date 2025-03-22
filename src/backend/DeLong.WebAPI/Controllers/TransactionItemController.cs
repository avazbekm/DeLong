using DeLong.Service.DTOs.TransactionItems;
using DeLong.Service.Interfaces;
using DeLong.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DeLong.WebAPI.Controllers;

public class TransactionItemController : BaseController
{
    private readonly ITransactionItemService transactionItemService;

    public TransactionItemController(ITransactionItemService transactionItemService)
    {
        this.transactionItemService = transactionItemService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> AddAsync(TransactionItemCreationDto dto)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await this.transactionItemService.AddAsync(dto)
        });

    [HttpPut("update")]
    public async Task<IActionResult> UpdateAsync([FromBody] TransactionItemUpdateDto dto)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await this.transactionItemService.ModifyAsync(dto)
        });

    [HttpDelete("remove/{id:long}")]
    public async Task<IActionResult> DestroyAsync(long id)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await this.transactionItemService.RemoveAsync(id)
        });

    [HttpGet("get/{id:long}")]
    public async Task<IActionResult> GetByIdAsync(long id)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await this.transactionItemService.RetrieveByIdAsync(id)
        });

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllAsync()
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await this.transactionItemService.RetrieveAllAsync()
        });

    [HttpGet("get-by-transaction/{transactionId:long}")]
    public async Task<IActionResult> GetByTransactionIdAsync(long transactionId)
    {
        // Hozircha service da bu metod yo‘q, shuning uchun placeholder qo‘yildi
        return Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = null
        });
    }
}