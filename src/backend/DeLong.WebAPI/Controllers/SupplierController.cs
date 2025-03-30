using DeLong.Application.DTOs.Suppliers;
using DeLong.Domain.Configurations;
using DeLong.Service.Interfaces;
using DeLong.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DeLong.WebAPI.Controllers
{
    public class SupplierController : BaseController
    {
        private readonly ISupplierService supplierService;
        public SupplierController(ISupplierService supplierService)
        {
            this.supplierService = supplierService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> AddAsync(SupplierCreationDto dto)
            => Ok(new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = await this.supplierService.AddAsync(dto)
            });

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAsync(SupplierUpdateDto dto)
            => Ok(new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = await this.supplierService.ModifyAsync(dto)
            });

        [HttpDelete("delete/{id:long}")]
        public async Task<IActionResult> DeleteAsync(long id)
            => Ok(new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = await this.supplierService.RemoveAsync(id)
            });

        [HttpGet("get/{id:long}")]
        public async Task<IActionResult> GetByIdAsync(long id)
            => Ok(new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = await this.supplierService.RetrieveByIdAsync(id)
            });

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllsync()
            => Ok(new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = await this.supplierService.RetrieveAllAsync()
            });
    }
}
