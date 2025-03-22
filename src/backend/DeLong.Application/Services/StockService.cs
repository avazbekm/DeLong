using AutoMapper;
using DeLong.Application.DTOs.Stocks;
using DeLong.Application.Exceptions;
using DeLong.Application.Extensions;
using DeLong.Application.Interfaces;
using DeLong.Domain.Configurations;
using DeLong.Domain.Entities;
using DeLong.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeLong.Service.Services;

public class StockService : IStockService
{
    private readonly IMapper mapper;
    private readonly IRepository<Stock> stockRepository;
    public StockService(IRepository<Stock> stockRepository, IMapper mapper)
    {
        this.mapper = mapper;
        this.stockRepository = stockRepository;
    }

    public async ValueTask<StockResultDto> AddAsync(StockCreationDto dto)
    {
        Stock existStocks = await this.stockRepository.GetAsync(u => u.ProductId.Equals(dto.ProductId));
        if (existStocks is not null)
            throw new AlreadyExistException($"This Stock is already exists with ProductId = {dto.ProductId}");

        var mappedStocks = this.mapper.Map<Stock>(dto);
        await this.stockRepository.CreateAsync(mappedStocks);
        await this.stockRepository.SaveChanges();

        var result = this.mapper.Map<StockResultDto>(mappedStocks);
        return result;
    }

    public async ValueTask<StockResultDto> ModifyAsync(StockUpdateDto dto)
    {
        Stock existStock = await this.stockRepository.GetAsync(u => u.Id.Equals(dto.Id))
            ?? throw new NotFoundException($"This Stock is not found with ID = {dto.Id}");

        this.mapper.Map(dto, existStock);
        this.stockRepository.Update(existStock);
        await this.stockRepository.SaveChanges();

        var result = this.mapper.Map<StockResultDto>(existStock);
        return result;
    }

    public async ValueTask<bool> RemoveAsync(long id)
    {
        Stock existStocks = await this.stockRepository.GetAsync(u => u.Id.Equals(id))
            ?? throw new NotFoundException($"This Stock is not found with ID = {id}");

        this.stockRepository.Delete(existStocks);
        await this.stockRepository.SaveChanges();
        return true;
    }

    public async ValueTask<StockResultDto> RetrieveByIdAsync(long id)
    {
        Stock existStocks = await this.stockRepository.GetAsync(u => u.Id.Equals(id))
            ?? throw new NotFoundException($"This Stock is not found with ID = {id}");

        var result = this.mapper.Map<StockResultDto>(existStocks);
        return result;
    }

    public async ValueTask<IEnumerable<StockResultDto>> RetrieveAllAsync(PaginationParams @params, Filter filter, string search = null)
    {
        var stocks = await this.stockRepository.GetAll()
            .ToPaginate(@params)
            .OrderBy(filter)
            .ToListAsync();

        var result = stocks.Where(stock => stock.Id.ToString().Contains(search, StringComparison.OrdinalIgnoreCase));
        var mappedStocks = this.mapper.Map<List<StockResultDto>>(result);
        return mappedStocks;
    }

    public async ValueTask<IEnumerable<StockResultDto>> RetrieveAllAsync()
    {
        var stocks = await this.stockRepository.GetAll()
            .ToListAsync();
        var result = this.mapper.Map<IEnumerable<StockResultDto>>(stocks);
        return result;
    }
}
