using AutoMapper;
using DeLong.Domain.Entities;
using DeLong.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using DeLong.Service.DTOs.Prices;
using DeLong.Domain.Configurations;
using DeLong.Application.Exceptions;
using DeLong.Application.Extensions;
using DeLong.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeLong.Service.Services;

public class PriceService : AuditableService, IPriceServer
{
    private readonly IMapper _mapper;
    private readonly IRepository<Price> _priceRepository;

    public PriceService(IRepository<Price> priceRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(httpContextAccessor)
    {
        _mapper = mapper;
        _priceRepository = priceRepository;
    }

    public async ValueTask<PriceResultDto> AddAsync(PriceCreationDto dto)
    {
        var existPrices = await _priceRepository.GetAsync(u =>
            u.ProductId.Equals(dto.ProductId) &&
            u.CostPrice.Equals(dto.CostPrice) &&
            u.SellingPrice.Equals(dto.SellingPrice) &&
            !u.IsDeleted);
        if (existPrices is not null)
            throw new AlreadyExistException($"This Price is already exists with ProductId = {dto.ProductId}");

        var mappedPrices = _mapper.Map<Price>(dto);
        SetCreatedFields(mappedPrices); // Auditable maydonlarni qo‘shish

        await _priceRepository.CreateAsync(mappedPrices);
        await _priceRepository.SaveChanges();

        return _mapper.Map<PriceResultDto>(mappedPrices);
    }

    public async ValueTask<PriceResultDto> ModifyAsync(PriceUpdateDto dto)
    {
        var existPrices = await _priceRepository.GetAsync(u => u.Id.Equals(dto.Id) && !u.IsDeleted)
            ?? throw new NotFoundException($"This Price is not found with ID = {dto.Id}");

        _mapper.Map(dto, existPrices);
        SetUpdatedFields(existPrices); // Auditable maydonlarni yangilash

        _priceRepository.Update(existPrices);
        await _priceRepository.SaveChanges();

        return _mapper.Map<PriceResultDto>(existPrices);
    }

    public async ValueTask<bool> RemoveAsync(long id)
    {
        var existPrices = await _priceRepository.GetAsync(u => u.Id.Equals(id) && !u.IsDeleted)
            ?? throw new NotFoundException($"This Price is not found with ID = {id}");

        existPrices.IsDeleted = true; // Soft delete
        SetUpdatedFields(existPrices); // Auditable maydonlarni yangilash

        _priceRepository.Update(existPrices);
        await _priceRepository.SaveChanges();
        return true;
    }

    public async ValueTask<PriceResultDto> RetrieveByIdAsync(long id)
    {
        var existPrices = await _priceRepository.GetAsync(u => u.Id.Equals(id) && !u.IsDeleted)
            ?? throw new NotFoundException($"This Price is not found with ID = {id}");

        return _mapper.Map<PriceResultDto>(existPrices);
    }

    public async ValueTask<IEnumerable<PriceResultDto>> RetrieveAllAsync()
    {
        var prices = await _priceRepository.GetAll(p => !p.IsDeleted)
            .ToListAsync();
        return _mapper.Map<IEnumerable<PriceResultDto>>(prices);
    }

    public async ValueTask<IEnumerable<PriceResultDto>> RetrieveAllAsync(long productId)
    {
        var prices = await _priceRepository.GetAll(p => p.ProductId.Equals(productId) && !p.IsDeleted)
            .ToListAsync();
        return _mapper.Map<IEnumerable<PriceResultDto>>(prices);
    }

    // Pagination metodi ishlatilmagan, agar kerak bo‘lsa qo‘shish mumkin
    public async ValueTask<IEnumerable<PriceResultDto>> RetrieveAllAsync(PaginationParams @params, Filter filter, string search = null)
    {
        var pricesQuery = _priceRepository.GetAll(p => !p.IsDeleted)
            .ToPaginate(@params)
            .OrderBy(filter);

        if (!string.IsNullOrEmpty(search))
        {
            pricesQuery = pricesQuery.Where(price => price.Id.ToString().Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        var prices = await pricesQuery.ToListAsync();
        return _mapper.Map<IEnumerable<PriceResultDto>>(prices);
    }
}