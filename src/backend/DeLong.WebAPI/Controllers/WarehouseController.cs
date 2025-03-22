using DeLong.Application.DTOs.Warehouses;
using DeLong.Service.Interfaces;
using DeLong.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DeLong.WebAPI.Controllers
{
    public class WarehouseController : BaseController
    {
        private readonly IWarehouseService warehouseService;
        public WarehouseController(IWarehouseService warehouseService)
        {
            this.warehouseService = warehouseService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> AddAsync(WarehouseCreationDto dto)
            => Ok(new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = await this.warehouseService.AddAsync(dto)
            });

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAsync(WarehouseUpdatedDto dto)
            => Ok(new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = await this.warehouseService.ModifyAsync(dto)
            });

        [HttpDelete("delete/{id:long}")]
        public async Task<IActionResult> DeleteAsync(long id)
            => Ok(new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = await this.warehouseService.RemoveAsync(id)
            });

        [HttpDelete("remove/{id:long}")]
        public async Task<IActionResult> DestroyAsync(long id)
            => Ok(new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = await this.warehouseService.RemoveAsync(id)
            });

        [HttpGet("get/{id:long}")]
        public async Task<IActionResult> GetByIdAsync(long id)
            => Ok(new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = await this.warehouseService.RetrieveByIdAsync(id)
            });

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllsync()
            => Ok(new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = await this.warehouseService.RetrieveAllAsync()
            });
    }
}

