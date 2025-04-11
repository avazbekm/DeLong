using DeLong.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using DeLong.Service.Interfaces;
using DeLong.Service.DTOs.Branchs;
using Microsoft.AspNetCore.Authorization;

namespace DeLong.WebAPI.Controllers;

[Authorize] // Faqat autentifikatsiya qilinganlar uchun
public class BranchController : BaseController
{
    private readonly IBranchService _service;

    public BranchController(IBranchService service)
    {
        _service = service;
    }

    [HttpPost("create")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> AddAsync(BranchCreationDto dto)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _service.AddAsync(dto)
        });

    [HttpPut("update")]
    [Authorize(Roles = "Admin,admin")]
    public async Task<IActionResult> UpdateAsync(BranchUpdateDto dto)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _service.ModifyAsync(dto)
        });

    [HttpDelete("remove/{id:long}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DestroyAsync(long id)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _service.RemoveAsync(id)
        });

    [HttpGet("get/{id:long}")]
    public async Task<IActionResult> GetByIdAsync(long id)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _service.RetrieveByIdAsync(id)
        });

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllAsync()
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _service.RetrieveAllAsync()
        });


    [HttpGet("history/{branchId}")]
    [Authorize(Roles = "admin")]
    public async ValueTask<ActionResult<IEnumerable<BranchChangeHistoryDto>>> GetHistoryAsync([FromRoute] long branchId)
        => Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = await _service.GetChangeHistoryAsync(branchId)
        });

}