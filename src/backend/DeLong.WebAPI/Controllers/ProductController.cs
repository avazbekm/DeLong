using DeLong.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using DeLong.Service.Interfaces;
using DeLong.Domain.Configurations;
using DeLong.Application.DTOs.Products;

namespace DeLong.WebAPI.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductService productService;
        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> AddAsync(ProductCreationDto dto)
            => Ok(new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = await this.productService.AddAsync(dto)
            });

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAsync(ProductUpdateDto dto)
            => Ok(new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = await this.productService.ModifyAsync(dto)
            });

        [HttpDelete("delete/{id:long}")]
        public async Task<IActionResult> DeleteAsync(long id)
            => Ok(new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = await this.productService.RemoveAsync(id)
            });

        [HttpDelete("remove/{id:long}")]
        public async Task<IActionResult> DestroyAsync(long id)
            => Ok(new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = await this.productService.RemoveAsync(id)
            });

        [HttpGet("get/{id:long}")]
        public async Task<IActionResult> GetByIdAsync(long id)
            => Ok(new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = await this.productService.RetrieveByIdAsync(id)
            });

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllsync([FromQuery] PaginationParams @params, [FromQuery] Filter filter, string search)
            => Ok(new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = await this.productService.RetrieveAllAsync(@params, filter, search)
            });
       
        [HttpGet("get-allProducts")]
        public async Task<IActionResult> GetAllsync()
            => Ok(new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = await this.productService.RetrieveAllAsync()
            });

    }

}