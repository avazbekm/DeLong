using AutoMapper;
using DeLong.Domain.Entities;
using DeLong.Domain.Configurations;
using DeLong.Application.Exceptions;
using DeLong.Application.Extensions;
using DeLong.Application.Interfaces;
using DeLong.Service.DTOs.SaleItems;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace DeLong.Service.Services;

public class SaleItemService : AuditableService, ISaleItemService
{
    private readonly IMapper _mapper;
    private readonly IRepository<SaleItem> _saleItemRepository;

    public SaleItemService(IRepository<SaleItem> saleItemRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(httpContextAccessor)
    {
        _mapper = mapper;
        _saleItemRepository = saleItemRepository;
    }

    public async ValueTask<SaleItemResultDto> AddAsync(SaleItemCreationDto dto)
    {
        var mappedSaleItem = _mapper.Map<SaleItem>(dto);
        SetCreatedFields(mappedSaleItem); // Auditable maydonlarni qo‘shish

        await _saleItemRepository.CreateAsync(mappedSaleItem);
        await _saleItemRepository.SaveChanges();

        return _mapper.Map<SaleItemResultDto>(mappedSaleItem);
    }

    public async ValueTask<SaleItemResultDto> ModifyAsync(SaleItemUpdateDto dto)
    {
        var existSaleItem = await _saleItemRepository.GetAsync(s => s.Id == dto.Id && !s.IsDeleted)
            ?? throw new NotFoundException($"SaleItem not found with ID = {dto.Id}");

        _mapper.Map(dto, existSaleItem);
        SetUpdatedFields(existSaleItem); // Auditable maydonlarni yangilash

        _saleItemRepository.Update(existSaleItem);
        await _saleItemRepository.SaveChanges();

        return _mapper.Map<SaleItemResultDto>(existSaleItem);
    }

    public async ValueTask<bool> RemoveAsync(long id)
    {
        var existSaleItem = await _saleItemRepository.GetAsync(s => s.Id == id && !s.IsDeleted)
            ?? throw new NotFoundException($"SaleItem not found with ID = {id}");

        existSaleItem.IsDeleted = true; // Soft delete
        SetUpdatedFields(existSaleItem); // Auditable maydonlarni yangilash

        _saleItemRepository.Update(existSaleItem);
        await _saleItemRepository.SaveChanges();
        return true;
    }

    public async ValueTask<SaleItemResultDto> RetrieveByIdAsync(long id)
    {
        var existSaleItem = await _saleItemRepository.GetAsync(s => s.Id == id && !s.IsDeleted)
            ?? throw new NotFoundException($"SaleItem not found with ID = {id}");

        return _mapper.Map<SaleItemResultDto>(existSaleItem);
    }

    public async ValueTask<IEnumerable<SaleItemResultDto>> RetrieveAllAsync(PaginationParams @params, Filter filter)
    {
        var saleItems = await _saleItemRepository.GetAll(s => !s.IsDeleted)
            .ToPaginate(@params)
            .OrderBy(filter)
            .ToListAsync();

        return _mapper.Map<IEnumerable<SaleItemResultDto>>(saleItems);
    }

    public async ValueTask<IEnumerable<SaleItemResultDto>> RetrieveAllBySaleIdAsync(long saleId)
    {
        var saleItems = await _saleItemRepository.GetAll(s => s.SaleId.Equals(saleId) && !s.IsDeleted)
            .ToListAsync();

        return _mapper.Map<IEnumerable<SaleItemResultDto>>(saleItems);
    }
}