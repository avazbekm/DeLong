using DeLong.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using DeLong.Service.Interfaces;
using DeLong.Service.DTOs.Sale;

namespace DeLong.WebAPI.Controllers;

public class SaleController : BaseController
{
    private readonly ISaleService saleService;

    public SaleController(ISaleService saleService)
    {
        this.saleService = saleService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> AddAsync(SaleCreationDto dto)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await this.saleService.AddAsync(dto)
        });

    [HttpGet("get/{id:long}")]
    public async Task<IActionResult> GetByIdAsync(long id)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await this.saleService.RetrieveByIdAsync(id)
        });

    [HttpGet("get-all")] // Yangi endpoint qo‘shildi
    public async Task<IActionResult> GetAllAsync()
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await this.saleService.RetrieveAllAsync()
        });
}