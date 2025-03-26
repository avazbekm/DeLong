using DeLong.Service.DTOs.KursDollar;
using DeLong.Service.Interfaces;
using DeLong.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeLong.WebAPI.Controllers;

[Authorize] // Faqat autentifikatsiya qilinganlar uchun
public class KursDollarController : BaseController
{
    private readonly IKursDollarService _kursDollarService;

    public KursDollarController(IKursDollarService kursDollarService)
    {
        _kursDollarService = kursDollarService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> AddAsync(KursDollarCreationDto dto)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _kursDollarService.AddAsync(dto)
        });

    [HttpGet("get")]
    public async Task<IActionResult> GetByIdAsync()
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _kursDollarService.RetrieveByIdAsync()
        });

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllAsync()
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _kursDollarService.RetrieveAllAsync()
        });
}