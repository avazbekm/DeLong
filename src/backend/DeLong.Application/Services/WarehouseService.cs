using AutoMapper;
using DeLong.Application.DTOs.Warehouses;
using DeLong.Application.Exceptions;
using DeLong.Application.Extensions;
using DeLong.Application.Interfaces;
using DeLong.Domain.Configurations;
using DeLong.Domain.Entities;
using DeLong.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeLong.Service.Services;

public class WarehouseService : IWarehouseService
{
    private readonly IMapper mapper;
    private readonly IRepository<Warehouse> warehouseRepository;
    public WarehouseService(IRepository<Warehouse> warehouseRepository, IMapper mapper)
    {
        this.mapper = mapper;
        this.warehouseRepository = warehouseRepository;
    }

    public async ValueTask<WarehouseResultDto> AddAsync(WarehouseCreationDto dto)
    {
        Warehouse existWarehouses = await this.warehouseRepository.GetAsync(u => u.Name.Equals(dto.Name));
        if (existWarehouses is not null)
            throw new AlreadyExistException($"This Warehouse is already exists with ProductId = {dto.Name}");

        var mappedWarehouses = this.mapper.Map<Warehouse>(dto);
        await this.warehouseRepository.CreateAsync(mappedWarehouses);
        await this.warehouseRepository.SaveChanges();

        var result = this.mapper.Map<WarehouseResultDto>(mappedWarehouses);
        return result;
    }

    public async ValueTask<WarehouseResultDto> ModifyAsync(WarehouseUpdatedDto dto)
    {
        Warehouse existWarehouses = await this.warehouseRepository.GetAsync(u => u.Id.Equals(dto.Id))
            ?? throw new NotFoundException($"This Warehouse is not found with ID = {dto.Id}");

        this.mapper.Map(dto, existWarehouses);
        this.warehouseRepository.Update(existWarehouses);
        await this.warehouseRepository.SaveChanges();

        var result = this.mapper.Map<WarehouseResultDto>(existWarehouses);
        return result;
    }

    public async ValueTask<bool> RemoveAsync(long id)
    {
        Warehouse existWarehouses = await this.warehouseRepository.GetAsync(u => u.Id.Equals(id))
            ?? throw new NotFoundException($"This Warehouse is not found with ID = {id}");

        this.warehouseRepository.Delete(existWarehouses);
        await this.warehouseRepository.SaveChanges();
        return true;
    }

    public async ValueTask<WarehouseResultDto> RetrieveByIdAsync(long id)
    {
        Warehouse existWarehouses = await this.warehouseRepository.GetAsync(u => u.Id.Equals(id))
            ?? throw new NotFoundException($"This Warehouse is not found with ID = {id}");

        var result = this.mapper.Map<WarehouseResultDto>(existWarehouses);
        return result;
    }

    public async ValueTask<IEnumerable<WarehouseResultDto>> RetrieveAllAsync(PaginationParams @params, Filter filter, string search = null)
    {
        var warehouses = await this.warehouseRepository.GetAll()
            .ToPaginate(@params)
            .OrderBy(filter)
            .ToListAsync();

        var result = warehouses.Where(warehouse => warehouse.Id.ToString().Contains(search, StringComparison.OrdinalIgnoreCase));
        var mappedWarehouses = this.mapper.Map<List<WarehouseResultDto>>(result);
        return mappedWarehouses;
    }

    public async ValueTask<IEnumerable<WarehouseResultDto>> RetrieveAllAsync()
    {
        var warehouses = await this.warehouseRepository.GetAll()
            .ToListAsync();
        var result = this.mapper.Map<IEnumerable<WarehouseResultDto>>(warehouses);
        return result;
    }

}
