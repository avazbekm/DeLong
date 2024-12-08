using DeLong.Application.DTOs.CashRegisters;
using DeLong.Application.DTOs.Customers;
using DeLong.Domain.Configurations;
using DeLong.Service.Interfaces;
using DeLong.WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeLong.WebAPI.Controllers
{
    public class CashRegisterController : BaseController
    {
        private readonly ICashRegisterService cashRegisterService;
        public CashRegisterController(ICashRegisterService cashRegisterService)
        {
            this.cashRegisterService = cashRegisterService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> AddAsync(CashRegisterCreationDto dto)
            => Ok(new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = await this.cashRegisterService.AddAsync(dto)
            });

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAsync(CashRegisterUpdateDto dto)
            => Ok(new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = await this.cashRegisterService.ModifyAsync(dto)
            });

        [HttpDelete("delete/{id:long}")]
        public async Task<IActionResult> DeleteAsync(long id)
            => Ok(new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = await this.cashRegisterService.RemoveAsync(id)
            });

        [HttpDelete("remove/{id:long}")]
        public async Task<IActionResult> DestroyAsync(long id)
            => Ok(new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = await this.cashRegisterService.RemoveAsync(id)
            });

        [HttpGet("get/{id:long}")]
        public async Task<IActionResult> GetByIdAsync(long id)
            => Ok(new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = await this.cashRegisterService.RetrieveByIdAsync(id)
            });

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllsync([FromQuery] PaginationParams @params, Filter filter)
            => Ok(new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = cashRegisterService.RetrieveAllAsync(@params, filter)
            });
    }
}
