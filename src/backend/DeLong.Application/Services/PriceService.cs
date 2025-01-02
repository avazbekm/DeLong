using AutoMapper;
using DeLong.Application.Exceptions;
using DeLong.Application.Extensions;
using DeLong.Application.Interfaces;
using DeLong.Domain.Configurations;
using DeLong.Domain.Entities;
using DeLong.Service.DTOs.Prices;
using DeLong.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeLong.Service.Services;

public class PriceService:IPriceServer
{
    private readonly IMapper mapper;
    private readonly IRepository<Price> priceRepository;
    public PriceService(IRepository<Price> priceRepository, IMapper mapper)
    {
        this.mapper = mapper;
        this.priceRepository = priceRepository;
    }

    public async ValueTask<PriceResultDto> AddAsync(PriceCreationDto dto)
    {
        Price existPrices = await this.priceRepository.GetAsync(u => u.ProductId.Equals(dto.ProductId));
        if (existPrices is not null)
            throw new AlreadyExistException($"This Price is already exists with ProductId = {dto.ProductId}");

        var mappedPrices = this.mapper.Map<Price>(dto);
        await this.priceRepository.CreateAsync(mappedPrices);
        await this.priceRepository.SaveChanges();

        var result = this.mapper.Map<PriceResultDto>(mappedPrices);
        return result;
    }

    public async ValueTask<PriceResultDto> ModifyAsync(PriceUpdateDto dto)
    {
        Price existPrices = await this.priceRepository.GetAsync(u => u.Id.Equals(dto.Id))
            ?? throw new NotFoundException($"This Price is not found with ID = {dto.Id}");

        this.mapper.Map(dto, existPrices);
        this.priceRepository.Update(existPrices);
        await this.priceRepository.SaveChanges();

        var result = this.mapper.Map<PriceResultDto>(existPrices);
        return result;
    }

    public async ValueTask<bool> RemoveAsync(long id)
    {
        Price existPrices = await this.priceRepository.GetAsync(u => u.Id.Equals(id))
            ?? throw new NotFoundException($"This Price is not found with ID = {id}");

        this.priceRepository.Delete(existPrices);
        await this.priceRepository.SaveChanges();
        return true;
    }

    public async ValueTask<PriceResultDto> RetrieveByIdAsync(long id)
    {
        Price existPrices = await this.priceRepository.GetAsync(u => u.Id.Equals(id))
            ?? throw new NotFoundException($"This Price is not found with ID = {id}");

        var result = this.mapper.Map<PriceResultDto>(existPrices);
        return result;
    }

    public async ValueTask<IEnumerable<PriceResultDto>> RetrieveAllAsync(PaginationParams @params, Filter filter, string search = null)
    {
        var prices = await this.priceRepository.GetAll()
            .ToPaginate(@params)
            .OrderBy(filter)
            .ToListAsync();

        var result = prices.Where(price => price.Id.ToString().Contains(search, StringComparison.OrdinalIgnoreCase));
        var mappedPrices = this.mapper.Map<List<PriceResultDto>>(result);
        return mappedPrices;
    }

    public async ValueTask<IEnumerable<PriceResultDto>> RetrieveAllAsync()
    {
        var prices = await this.priceRepository.GetAll()
            .ToListAsync();
        var result = this.mapper.Map<IEnumerable<PriceResultDto>>(prices);
        return result;
    }
}
