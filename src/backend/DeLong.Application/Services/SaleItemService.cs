using AutoMapper;
using DeLong.Domain.Entities;
using DeLong.Service.Interfaces;
using DeLong.Domain.Configurations;
using DeLong.Application.Exceptions;
using DeLong.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using DeLong.Application.Extensions;
using DeLong.Service.DTOs.SaleItems;

namespace DeLong.Service.Services;

public class SaleItemService : ISaleItemService
{
    private readonly IMapper mapper;
    private readonly IRepository<SaleItem> saleItemRepository;

    public SaleItemService(IRepository<SaleItem> saleItemRepository, IMapper mapper)
    {
        this.mapper = mapper;
        this.saleItemRepository = saleItemRepository;
    }

    public async ValueTask<SaleItemResultDto> AddAsync(SaleItemCreationDto dto)
    {
        var mappedSaleItem = this.mapper.Map<SaleItem>(dto);
        await this.saleItemRepository.CreateAsync(mappedSaleItem);
        await this.saleItemRepository.SaveChanges();

        return this.mapper.Map<SaleItemResultDto>(mappedSaleItem);
    }

    public async ValueTask<SaleItemResultDto> ModifyAsync(SaleItemUpdateDto dto)
    {
        var existSaleItem = await this.saleItemRepository.GetAsync(s => s.Id == dto.Id)
            ?? throw new NotFoundException($"SaleItem not found with ID = {dto.Id}");

        this.mapper.Map(dto, existSaleItem);
        this.saleItemRepository.Update(existSaleItem);
        await this.saleItemRepository.SaveChanges();

        return this.mapper.Map<SaleItemResultDto>(existSaleItem);
    }

    public async ValueTask<bool> RemoveAsync(long id)
    {
        var existSaleItem = await this.saleItemRepository.GetAsync(s => s.Id == id)
            ?? throw new NotFoundException($"SaleItem not found with ID = {id}");

        this.saleItemRepository.Delete(existSaleItem);
        await this.saleItemRepository.SaveChanges();
        return true;
    }

    public async ValueTask<SaleItemResultDto> RetrieveByIdAsync(long id)
    {
        var existSaleItem = await this.saleItemRepository.GetAsync(s => s.Id == id)
            ?? throw new NotFoundException($"SaleItem not found with ID = {id}");

        return this.mapper.Map<SaleItemResultDto>(existSaleItem);
    }

    public async ValueTask<IEnumerable<SaleItemResultDto>> RetrieveAllAsync(PaginationParams @params, Filter filter)
    {
        var saleItems = await this.saleItemRepository.GetAll()
            .ToPaginate(@params)
            .OrderBy(filter)
            .ToListAsync();

        return this.mapper.Map<IEnumerable<SaleItemResultDto>>(saleItems);
    }

    public async ValueTask<IEnumerable<SaleItemResultDto>> RetrieveAllBySaleIdAsync(long saleId)
    {
        var saleItems = await this.saleItemRepository.GetAll().Where(s => s.SaleId.Equals(saleId))
            .ToListAsync();

        return this.mapper.Map<IEnumerable<SaleItemResultDto>>(saleItems);
    }
}
