﻿using DeLong.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using DeLong.Service.Interfaces;
using DeLong.Service.DTOs.Prices;

namespace DeLong.WebAPI.Controllers;

public class PriceController : BaseController
{
    private readonly IPriceServer priceService;
    public PriceController(IPriceServer priceService)
    {
        this.priceService = priceService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> AddAsync(PriceCreationDto dto)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await this.priceService.AddAsync(dto)
        });

    [HttpPut("update")]
    public async Task<IActionResult> UpdateAsync(PriceUpdateDto dto)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await this.priceService.ModifyAsync(dto)
        });

    [HttpDelete("delete/{id:long}")]
    public async Task<IActionResult> DeleteAsync(long id)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await this.priceService.RemoveAsync(id)
        });

    [HttpDelete("remove/{id:long}")]
    public async Task<IActionResult> DestroyAsync(long id)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await this.priceService.RemoveAsync(id)
        });

    [HttpGet("get/{id:long}")]
    public async Task<IActionResult> GetByIdAsync(long id)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await this.priceService.RetrieveByIdAsync(id)
        });

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllsync()
    => Ok(new Response
    {
        StatusCode = 200,
        Message = "Success",
        Data = await this.priceService.RetrieveAllAsync()
    });

    [HttpGet("get-allProductId")]
    public async Task<IActionResult> GetAllsync(long productId)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await this.priceService.RetrieveAllAsync(productId)
        });
}
