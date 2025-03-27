using AutoMapper;
using DeLong.Application.DTOs.Warehouses;
using DeLong.Application.Exceptions;
using DeLong.Application.Extensions;
using DeLong.Application.Interfaces;
using DeLong.Domain.Configurations;
using DeLong.Domain.Entities;
using DeLong.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DeLong.Service.Services;

public class WarehouseService : AuditableService, IWarehouseService
{
    private readonly IMapper _mapper;
    private readonly IRepository<Warehouse> _warehouseRepository;

    public WarehouseService(IRepository<Warehouse> warehouseRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(httpContextAccessor)
    {
        _mapper = mapper;
        _warehouseRepository = warehouseRepository;
    }

    public async ValueTask<WarehouseResultDto> AddAsync(WarehouseCreationDto dto)
    {
        var existWarehouse = await _warehouseRepository.GetAsync(w => w.Name.Equals(dto.Name) && !w.IsDeleted);
        if (existWarehouse != null)
            throw new AlreadyExistException($"This warehouse already exists with Name = {dto.Name}");

        var mappedWarehouse = _mapper.Map<Warehouse>(dto);
        SetCreatedFields(mappedWarehouse); // Auditable maydonlarni qo‘shish

        await _warehouseRepository.CreateAsync(mappedWarehouse);
        await _warehouseRepository.SaveChanges();
        return _mapper.Map<WarehouseResultDto>(mappedWarehouse);
    }

    public async ValueTask<WarehouseResultDto> ModifyAsync(WarehouseUpdatedDto dto)
    {
        var existWarehouse = await _warehouseRepository.GetAsync(w => w.Id.Equals(dto.Id) && !w.IsDeleted)
            ?? throw new NotFoundException($"This warehouse is not found with ID = {dto.Id}");

        _mapper.Map(dto, existWarehouse);
        SetUpdatedFields(existWarehouse); // Auditable maydonlarni yangilash

        _warehouseRepository.Update(existWarehouse);
        await _warehouseRepository.SaveChanges();
        return _mapper.Map<WarehouseResultDto>(existWarehouse);
    }

    public async ValueTask<bool> RemoveAsync(long id)
    {
        var existWarehouse = await _warehouseRepository.GetAsync(w => w.Id.Equals(id) && !w.IsDeleted)
            ?? throw new NotFoundException($"This warehouse is not found with ID = {id}");

        existWarehouse.IsDeleted = true; // Soft delete
        SetUpdatedFields(existWarehouse); // Auditable maydonlarni yangilash

        _warehouseRepository.Update(existWarehouse);
        await _warehouseRepository.SaveChanges();
        return true;
    }

    public async ValueTask<WarehouseResultDto> RetrieveByIdAsync(long id)
    {
        var existWarehouse = await _warehouseRepository.GetAsync(w => w.Id.Equals(id) && !w.IsDeleted)
            ?? throw new NotFoundException($"This warehouse is not found with ID = {id}");

        return _mapper.Map<WarehouseResultDto>(existWarehouse);
    }

    public async ValueTask<IEnumerable<WarehouseResultDto>> RetrieveAllAsync(PaginationParams @params, Filter filter, string search = null)
    {
        var query = _warehouseRepository.GetAll(w => !w.IsDeleted);

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(w => w.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                     w.Id.ToString().Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        var warehouses = await query
            .ToPaginate(@params)
            .OrderBy(filter)
            .ToListAsync();

        return _mapper.Map<IEnumerable<WarehouseResultDto>>(warehouses);
    }

    public async ValueTask<IEnumerable<WarehouseResultDto>> RetrieveAllAsync()
    {
        var warehouses = await _warehouseRepository.GetAll(w => !w.IsDeleted)
            .ToListAsync();
        return _mapper.Map<IEnumerable<WarehouseResultDto>>(warehouses);
    }
}