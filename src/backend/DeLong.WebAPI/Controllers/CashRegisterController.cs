using DeLong.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using DeLong.Service.Interfaces;
using DeLong.Application.DTOs.CashRegisters;

namespace DeLong.WebAPI.Controllers
{
    public class CashRegisterController : BaseController
    {
        private readonly ICashRegisterService _service;

        public CashRegisterController(ICashRegisterService service)
        {
            _service = service;
        }

        [HttpPost("create")]
        public async Task<IActionResult> AddAsync(CashRegisterCreationDto dto)
            => Ok(new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = await _service.AddAsync(dto)
            });

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAsync(CashRegisterUpdateDto dto)
            => Ok(new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = await _service.ModifyAsync(dto)
            });

        [HttpDelete("remove/{id:long}")]
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

        [HttpGet("get-by-user/{userId:long}")]
        public async Task<IActionResult> GetByUserIdAsync(long userId)
            => Ok(new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = await _service.RetrieveAllByUserIdAsync(userId)
            });

        [HttpGet("get-by-warehouse/{warehouseId:long}")]
        public async Task<IActionResult> GetByWarehouseIdAsync(long warehouseId)
            => Ok(new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = await _service.RetrieveAllByWarehouseIdAsync(warehouseId)
            });

        [HttpGet("get-open")]
        public async Task<IActionResult> GetOpenRegistersAsync()
            => Ok(new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = await _service.RetrieveOpenRegistersAsync()
            });
    }
}