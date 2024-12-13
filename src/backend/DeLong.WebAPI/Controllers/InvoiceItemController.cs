using DeLong.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using DeLong.Service.Interfaces;
using DeLong.Domain.Configurations;
using DeLong.Application.DTOs.InvoiceItems;

namespace DeLong.WebAPI.Controllers
{
    public class InvoiceItemController : BaseController
    {
        private readonly IInvoiceItemService invoiceItemService;
        public InvoiceItemController(IInvoiceItemService invoiceItemService)
        {
            this.invoiceItemService = invoiceItemService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> AddAsync(InvoiceItemCreationDto dto)
            => Ok(new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = await this.invoiceItemService.AddAsync(dto)
            });

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAsync(InvoiceItemUpdateDto dto)
            => Ok(new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = await this.invoiceItemService.ModifyAsync(dto)
            });

        [HttpDelete("delete/{id:long}")]
        public async Task<IActionResult> DeleteAsync(long id)
            => Ok(new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = await this.invoiceItemService.RemoveAsync(id)
            });

        [HttpDelete("remove/{id:long}")]
        public async Task<IActionResult> DestroyAsync(long id)
            => Ok(new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = await this.invoiceItemService.RemoveAsync(id)
            });

        [HttpGet("get/{id:long}")]
        public async Task<IActionResult> GetByIdAsync(long id)
            => Ok(new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = await this.invoiceItemService.RetrieveByIdAsync(id)
            });

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllsync([FromQuery] PaginationParams @params, [FromQuery] Filter filter, string search)
            => Ok(new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = await this.invoiceItemService.RetrieveAllAsync(@params, filter, search)
            });
    }

}

