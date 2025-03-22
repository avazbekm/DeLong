using AutoMapper;
using DeLong.Application.Exceptions;
using DeLong.Application.Interfaces;
using DeLong.Domain.Entities;
using DeLong.Service.DTOs.Sale;
using DeLong.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeLong.Service.Services;

public class SaleService : ISaleService
{
    private readonly IMapper mapper;
    private readonly IRepository<Sale> saleRepository;

    public SaleService(IRepository<Sale> saleRepository, IMapper mapper)
    {
        this.mapper = mapper;
        this.saleRepository = saleRepository;
    }

    public async ValueTask<SaleResultDto> AddAsync(SaleCreationDto dto)
    {
        var newSale = this.mapper.Map<Sale>(dto);
        await this.saleRepository.CreateAsync(newSale);
        await this.saleRepository.SaveChanges();
        return this.mapper.Map<SaleResultDto>(newSale);
    }

    public async ValueTask<SaleResultDto> RetrieveByIdAsync(long id)
    {
        var sale = await this.saleRepository.GetAsync(s => s.Id == id)
            ?? throw new NotFoundException($"Sale not found with ID = {id}");

        return this.mapper.Map<SaleResultDto>(sale);
    }

    public async ValueTask<IEnumerable<SaleResultDto>> RetrieveAllAsync()
    {
        var sales = await this.saleRepository.GetAll().ToListAsync();
        return this.mapper.Map<IEnumerable<SaleResultDto>>(sales);
    }
}