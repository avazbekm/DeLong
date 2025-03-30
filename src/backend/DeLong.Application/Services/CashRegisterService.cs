using AutoMapper;
using DeLong.Domain.Entities;
using Microsoft.AspNetCore.Http;
using DeLong.Service.Interfaces;
using DeLong.Application.Exceptions;
using DeLong.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using DeLong.Application.DTOs.CashRegisters;

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
            UserId = GetCurrentUserId(),
            BranchId = GetCurrentBranchId(),
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
        var branchId = GetCurrentBranchId();

        var existCashRegister = await _repository.GetAsync(r => r.Id == id && !r.IsDeleted && r.BranchId.Equals(branchId))
            ?? throw new NotFoundException($"CashRegister not found with ID = {id}");

        return _mapper.Map<CashRegisterResultDto>(existCashRegister);
    }

    public async ValueTask<IEnumerable<CashRegisterResultDto>> RetrieveAllAsync()
    {
        var branchId = GetCurrentBranchId();
        var cashRegisters = await _repository.GetAll(r => !r.IsDeleted && r.BranchId.Equals(branchId))
            .ToListAsync();

        return _mapper.Map<IEnumerable<CashRegisterResultDto>>(cashRegisters);
    }

    public async ValueTask<IEnumerable<CashRegisterResultDto>> RetrieveAllByUserIdAsync(long userId)
    {

        var branchId = GetCurrentBranchId();
        var cashRegisters = await _repository.GetAll(r => r.UserId == userId && !r.IsDeleted && r.BranchId.Equals(branchId))
            .ToListAsync();

        return _mapper.Map<IEnumerable<CashRegisterResultDto>>(cashRegisters);
    }

    public async ValueTask<IEnumerable<CashRegisterResultDto>> RetrieveAllByWarehouseIdAsync(long branchId)
    {
        var cashRegisters = await _repository.GetAll(r => r.BranchId == branchId && !r.IsDeleted)
            .ToListAsync();

        return _mapper.Map<IEnumerable<CashRegisterResultDto>>(cashRegisters);
    }

    public async ValueTask<IEnumerable<CashRegisterResultDto>> RetrieveOpenRegistersAsync()
    {
        var branchId = GetCurrentBranchId();
        var openRegisters = await _repository.GetAll(r => r.ClosedAt == null && !r.IsDeleted && r.BranchId.Equals(branchId))
            .ToListAsync();

        return _mapper.Map<IEnumerable<CashRegisterResultDto>>(openRegisters);
    }
}