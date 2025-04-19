using DeLong.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using DeLong.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using DeLong.Service.DTOs.CreditorDebts;

namespace DeLong.WebAPI.Controllers;

[Authorize]
public class CreditorDebtController : BaseController
{
    private readonly ICreditorDebtService _creditorDebtService;

    public CreditorDebtController(ICreditorDebtService creditorDebtService)
    {
        _creditorDebtService = creditorDebtService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> AddAsync(CreditorDebtCreationDto dto)
    {
        try
        {
            var result = await _creditorDebtService.AddAsync(dto);
            return Ok(new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = result
            });
        }
        catch (Application.Exceptions.AlreadyExistException ex)
        {
            return BadRequest(new Response
            {
                StatusCode = 400,
                Message = ex.Message,
                Data = null
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new Response
            {
                StatusCode = 500,
                Message = "Internal server error: " + ex.Message,
                Data = null
            });
        }
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateAsync(CreditorDebtUpdateDto dto)
    {
        try
        {
            var result = await _creditorDebtService.ModifyAsync(dto);
            return Ok(new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = result
            });
        }
        catch (Application.Exceptions.NotFoundException ex)
        {
            return NotFound(new Response
            {
                StatusCode = 404,
                Message = ex.Message,
                Data = null
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new Response
            {
                StatusCode = 500,
                Message = "Internal server error: " + ex.Message,
                Data = null
            });
        }
    }

    [HttpDelete("remove/{id:long}")]
    public async Task<IActionResult> DestroyAsync(long id)
    {
        try
        {
            var result = await _creditorDebtService.RemoveAsync(id);
            return Ok(new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = result
            });
        }
        catch (Application.Exceptions.NotFoundException ex)
        {
            return NotFound(new Response
            {
                StatusCode = 404,
                Message = ex.Message,
                Data = null
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new Response
            {
                StatusCode = 500,
                Message = "Internal server error: " + ex.Message,
                Data = null
            });
        }
    }

    [HttpGet("get/{id:long}")]
    public async Task<IActionResult> GetByIdAsync(long id)
    {
        try
        {
            var result = await _creditorDebtService.RetrieveByIdAsync(id);
            return Ok(new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = result
            });
        }
        catch (Application.Exceptions.NotFoundException ex)
        {
            return NotFound(new Response
            {
                StatusCode = 404,
                Message = ex.Message,
                Data = null
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new Response
            {
                StatusCode = 500,
                Message = "Internal server error: " + ex.Message,
                Data = null
            });
        }
    }

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllAsync()
    {
        try
        {
            var result = await _creditorDebtService.RetrieveAllAsync();
            return Ok(new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = result
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new Response
            {
                StatusCode = 500,
                Message = "Internal server error: " + ex.Message,
                Data = null
            });
        }
    }
}