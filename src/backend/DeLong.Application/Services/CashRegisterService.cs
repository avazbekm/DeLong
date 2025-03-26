using AutoMapper;
using DeLong.Application.DTOs.CashRegisters;
using DeLong.Application.Exceptions;
using DeLong.Application.Interfaces;
using DeLong.Domain.Entities;
using DeLong.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DeLong.Service.Services;

#pragma warning disable // warninglarni o'chirish uchun
public class CashRegisterService : AuditableService, ICashRegisterService
{
    private readonly IRepository<CashRegister> _repository;
    private readonly IMapper _mapper;

    public CashRegisterService(IRepository<CashRegister> repository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(httpContextAccessor)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async ValueTask<CashRegisterResultDto> AddAsync(CashRegisterCreationDto dto)
    {
        var cashRegister = new CashRegister
        {
            UserId = dto.UserId,
            WarehouseId = dto.WarehouseId,
            UzsBalance = dto.UzsBalance,
            UzpBalance = dto.UzpBalance,
            UsdBalance = dto.UsdBalance,
            OpenedAt = DateTimeOffset.UtcNow, // Backend’da qo‘yiladi
            ClosedAt = null // Yangi kassa ochilganda null
        };

        SetCreatedFields(cashRegister); // Auditable maydonlarni qo‘shish
        await _repository.CreateAsync(cashRegister);
        await _repository.SaveChanges();

        return _mapper.Map<CashRegisterResultDto>(cashRegister);
    }

    public async ValueTask<CashRegisterResultDto> ModifyAsync(CashRegisterUpdateDto dto)
    {
        var existCashRegister = await _repository.GetAsync(r => r.Id == dto.Id && !r.IsDeleted)
            ?? throw new NotFoundException($"CashRegister not found with ID = {dto.Id}");

        _mapper.Map(dto, existCashRegister);
        SetUpdatedFields(existCashRegister); // Auditable maydonlarni yangilash

        _repository.Update(existCashRegister);
        await _repository.SaveChanges();

        return _mapper.Map<CashRegisterResultDto>(existCashRegister);
    }

    public async ValueTask<bool> RemoveAsync(long id)
    {
        var existCashRegister = await _repository.GetAsync(r => r.Id == id && !r.IsDeleted)
            ?? throw new NotFoundException($"CashRegister not found with ID = {id}");

        existCashRegister.IsDeleted = true; // Soft delete
        SetUpdatedFields(existCashRegister); // Auditable maydonlarni yangilash

        _repository.Update(existCashRegister); // Delete o‘rniga Update
        await _repository.SaveChanges();
        return true;
    }

    public async ValueTask<CashRegisterResultDto> RetrieveByIdAsync(long id)
    {
        var existCashRegister = await _repository.GetAsync(r => r.Id == id && !r.IsDeleted)
            ?? throw new NotFoundException($"CashRegister not found with ID = {id}");

        return _mapper.Map<CashRegisterResultDto>(existCashRegister);
    }

    public async ValueTask<IEnumerable<CashRegisterResultDto>> RetrieveAllAsync()
    {
        var cashRegisters = await _repository.GetAll(r => !r.IsDeleted)
            .ToListAsync();

        return _mapper.Map<IEnumerable<CashRegisterResultDto>>(cashRegisters);
    }

    public async ValueTask<IEnumerable<CashRegisterResultDto>> RetrieveAllByUserIdAsync(long userId)
    {
        var cashRegisters = await _repository.GetAll(r => r.UserId == userId && !r.IsDeleted)
            .ToListAsync();

        return _mapper.Map<IEnumerable<CashRegisterResultDto>>(cashRegisters);
    }

    public async ValueTask<IEnumerable<CashRegisterResultDto>> RetrieveAllByWarehouseIdAsync(long warehouseId)
    {
        var cashRegisters = await _repository.GetAll(r => r.WarehouseId == warehouseId && !r.IsDeleted)
            .ToListAsync();

        return _mapper.Map<IEnumerable<CashRegisterResultDto>>(cashRegisters);
    }

    public async ValueTask<IEnumerable<CashRegisterResultDto>> RetrieveOpenRegistersAsync()
    {
        var openRegisters = await _repository.GetAll(r => r.ClosedAt == null && !r.IsDeleted)
            .ToListAsync();

        return _mapper.Map<IEnumerable<CashRegisterResultDto>>(openRegisters);
    }
}