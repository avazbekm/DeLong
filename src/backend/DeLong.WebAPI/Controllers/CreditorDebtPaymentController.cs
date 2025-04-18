using DeLong.WebAPI.Models;
using DeLong.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DeLong.Service.DTOs.CreditorDebtPayments;

namespace DeLong.WebAPI.Controllers;

[Authorize]
public class CreditorDebtPaymentController : BaseController
{
    private readonly ICreditorDebtPaymentService _paymentService;

    public CreditorDebtPaymentController(ICreditorDebtPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> AddAsync(CreditorDebtPaymentCreationDto dto)
    {
        try
        {
            var result = await _paymentService.AddAsync(dto);
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
        catch (Application.Exceptions.CustomException ex)
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
    public async Task<IActionResult> UpdateAsync(CreditorDebtPaymentUpdateDto dto)
    {
        try
        {
            var result = await _paymentService.ModifyAsync(dto);
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
            var result = await _paymentService.RemoveAsync(id);
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
            var result = await _paymentService.RetrieveByIdAsync(id);
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
            var result = await _paymentService.RetrieveAllAsync();
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