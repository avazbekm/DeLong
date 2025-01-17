﻿using DeLong.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using DeLong.Service.Interfaces;
using DeLong.Domain.Configurations;
using DeLong.Application.DTOs.Customers;

namespace DeLong.WebAPI.Controllers;

public class CustomerController : BaseController
{
    private readonly ICustomerService customerService;
    public CustomerController(ICustomerService customerService)
    {
        this.customerService = customerService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> AddAsync(CustomerCreationDto dto)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await this.customerService.AddAsync(dto)
        });

    [HttpPut("update")]
    public async Task<IActionResult> UpdateAsync(CustomerUpdateDto dto)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await this.customerService.ModifyAsync(dto)
        });

    [HttpDelete("delete/{id:long}")]
    public async Task<IActionResult> DeleteAsync(long id)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await this.customerService.RemoveAsync(id)
        });

    [HttpDelete("remove/{id:long}")]
    public async Task<IActionResult> DestroyAsync(long id)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await this.customerService.RemoveAsync(id)
        });

    [HttpGet("get/{id:long}")]
    public async Task<IActionResult> GetByIdAsync(long id)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await this.customerService.RetrieveByIdAsync(id)
        });

    [HttpGet("get/INN")]
    public async Task<IActionResult> GetByInnAsync(int inn)
       => Ok(new Response
       {
           StatusCode = 200,
           Message = "Success",
           Data = await this.customerService.RetrieveByInnAsync(inn)
       });

    [HttpGet("get/Jshshir")]
    public async Task<IActionResult> GetByJshshirAsync(string jshshir)
       => Ok(new Response
       {
           StatusCode = 200,
           Message = "Success",
           Data = await this.customerService.RetrieveByJshshirAsync(jshshir)
       });

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllsync([FromQuery] PaginationParams @params, [FromQuery] Filter filter, string search)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await this.customerService.RetrieveAllAsync(@params, filter, search)
        }); 
    
    [HttpGet("get-allCustomers")]
    public async Task<IActionResult> GetAllsync()
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await this.customerService.RetrieveAllAsync()
        });

}