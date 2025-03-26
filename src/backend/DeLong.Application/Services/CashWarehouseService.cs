using AutoMapper;
using DeLong.Application.Exceptions;
using DeLong.Application.Interfaces;
using DeLong.Domain.Entities;
using DeLong.Service.DTOs.CashWarehouse;
using DeLong.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DeLong.Service.Services;

public class CashWarehouseService : AuditableService, ICashWarehouseService
{
    private readonly IRepository<CashWarehouse> _repository;
    private readonly IMapper _mapper;

    public CashWarehouseService(IRepository<CashWarehouse> repository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(httpContextAccessor)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async ValueTask<CashWarehouseResultDto> AddAsync(CashWarehouseCreationDto dto)
    {
        var cashWarehouse = _mapper.Map<CashWarehouse>(dto);
        SetCreatedFields(cashWarehouse); // Auditable maydonlarni qo‘shish

        await _repository.CreateAsync(cashWarehouse);
        await _repository.SaveChanges();

        return _mapper.Map<CashWarehouseResultDto>(cashWarehouse);
    }

    public async ValueTask<CashWarehouseResultDto> ModifyAsync(CashWarehouseUpdateDto dto)
    {
        var existCashWarehouse = await _repository.GetAsync(w => w.Id == dto.Id && !w.IsDeleted)
            ?? throw new NotFoundException($"CashWarehouse not found with ID = {dto.Id}");

        _mapper.Map(dto, existCashWarehouse);
        SetUpdatedFields(existCashWarehouse); // Auditable maydonlarni yangilash

        _repository.Update(existCashWarehouse);
        await _repository.SaveChanges();

        return _mapper.Map<CashWarehouseResultDto>(existCashWarehouse);
    }

    public async ValueTask<bool> RemoveAsync(long id)
    {
        var existCashWarehouse = await _repository.GetAsync(w => w.Id == id && !w.IsDeleted)
            ?? throw new NotFoundException($"CashWarehouse not found with ID = {id}");

        existCashWarehouse.IsDeleted = true; // Soft delete
        SetUpdatedFields(existCashWarehouse); // Auditable maydonlarni yangilash

        _repository.Update(existCashWarehouse);
        await _repository.SaveChanges();
        return true;
    }

    public async ValueTask<CashWarehouseResultDto> RetrieveByIdAsync()
    {
        // Id o‘rniga eng oxirgi qo‘shilgan zaxira omborini olamiz
        var latestCashWarehouse = await _repository.GetAll(w => !w.IsDeleted)
            .OrderByDescending(w => w.CreatedAt) // CreatedAt bo‘yicha eng so‘nggi
            .FirstOrDefaultAsync()
            ?? throw new NotFoundException("Hech qanday zaxira ombori topilmadi");

        return _mapper.Map<CashWarehouseResultDto>(latestCashWarehouse);
    }

    public async ValueTask<IEnumerable<CashWarehouseResultDto>> RetrieveAllAsync()
    {
        var cashWarehouses = await _repository.GetAll(w => !w.IsDeleted)
            .ToListAsync();

        return _mapper.Map<IEnumerable<CashWarehouseResultDto>>(cashWarehouses);
    }
}