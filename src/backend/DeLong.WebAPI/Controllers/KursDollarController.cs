using DeLong.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using DeLong.Service.Interfaces;
using DeLong.Service.DTOs.KursDollar;

namespace DeLong.WebAPI.Controllers;

public class KursDollarController : BaseController
{
    private readonly IKursDollarService kursDollarService;
    public KursDollarController(IKursDollarService kursDollarService)
    {
        this.kursDollarService = kursDollarService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> AddAsync(KursDollarCreationDto dto)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await this.kursDollarService.AddAsync(dto)
        });


    [HttpGet("get/{id:long}")]
    public async Task<IActionResult> GetByIdAsync(long id)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await this.kursDollarService.RetrieveByIdAsync(id)
        });

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllsync()
    => Ok(new Response
    {
        StatusCode = 200,
        Message = "Success",
        Data = await this.kursDollarService.RetrieveAllAsync()
    });
}

