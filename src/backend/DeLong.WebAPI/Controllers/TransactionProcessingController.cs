using DeLong.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using DeLong.Service.Interfaces;
using DeLong.Service.DTOs.ReceiveItems;
using Microsoft.AspNetCore.Authorization;

namespace DeLong.WebAPI.Controllers;

[Authorize]
public class TransactionProcessingController : BaseController
{
    private readonly ITransactionProcessingService _transactionProcessingService;

    public TransactionProcessingController(ITransactionProcessingService transactionProcessingService)
    {
        _transactionProcessingService = transactionProcessingService;
    }

    [HttpPost("process")]
    public async Task<IActionResult> ProcessTransactionAsync([FromBody] ProcessTransactionRequest request)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _transactionProcessingService.ProcessTransactionAsync(request.ReceiveItems, request.RequestId)
        });
}

public class ProcessTransactionRequest
{
    public List<ReceiveItemDto> ReceiveItems { get; set; }
    public Guid? RequestId { get; set; }
}