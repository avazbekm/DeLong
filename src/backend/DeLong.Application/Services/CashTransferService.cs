using AutoMapper;
using DeLong.Application.Exceptions;
using DeLong.Application.Interfaces;
using DeLong.Domain.Entities;
using DeLong.Service.DTOs.CashTransfers;
using DeLong.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DeLong.Service.Services;

public class CashTransferService : AuditableService, ICashTransferService
{
    private readonly IRepository<CashTransfer> _repository;
    private readonly IMapper _mapper;

    public CashTransferService(IRepository<CashTransfer> repository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(httpContextAccessor)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async ValueTask<CashTransferResultDto> AddAsync(CashTransferCreationDto dto)
    {
        var cashTransfer = _mapper.Map<CashTransfer>(dto);
        SetCreatedFields(cashTransfer); // Auditable maydonlarni qo‘shish
        cashTransfer.BranchId=GetCurrentBranchId(); // branchId aniqlab berilyapti

        await _repository.CreateAsync(cashTransfer);
        await _repository.SaveChanges();

        return _mapper.Map<CashTransferResultDto>(cashTransfer);
    }

    public async ValueTask<CashTransferResultDto> ModifyAsync(CashTransferUpdateDto dto)
    {
        var existCashTransfer = await _repository.GetAsync(t => t.Id == dto.Id && !t.IsDeleted)
            ?? throw new NotFoundException($"CashTransfer not found with ID = {dto.Id}");

        _mapper.Map(dto, existCashTransfer);
        SetUpdatedFields(existCashTransfer); // Auditable maydonlarni yangilash

        _repository.Update(existCashTransfer);
        await _repository.SaveChanges();

        return _mapper.Map<CashTransferResultDto>(existCashTransfer);
    }

    public async ValueTask<bool> RemoveAsync(long id)
    {
        var existCashTransfer = await _repository.GetAsync(t => t.Id == id && !t.IsDeleted)
            ?? throw new NotFoundException($"CashTransfer not found with ID = {id}");

        existCashTransfer.IsDeleted = true; // Soft delete
        SetUpdatedFields(existCashTransfer); // Auditable maydonlarni yangilash

        _repository.Update(existCashTransfer); // Delete o‘rniga Update
        await _repository.SaveChanges();
        return true;
    }

    public async ValueTask<CashTransferResultDto> RetrieveByIdAsync(long id)
    {
        var branchId = GetCurrentBranchId();
        var existCashTransfer = await _repository.GetAsync(t => t.Id == id && !t.IsDeleted && t.BranchId.Equals(branchId))
            ?? throw new NotFoundException($"CashTransfer not found with ID = {id}");

        return _mapper.Map<CashTransferResultDto>(existCashTransfer);
    }

    public async ValueTask<IEnumerable<CashTransferResultDto>> RetrieveAllAsync()
    {
        var branchId = GetCurrentBranchId();
        var cashTransfers = await _repository.GetAll(t => !t.IsDeleted && t.BranchId.Equals(branchId))
            .ToListAsync();

        return _mapper.Map<IEnumerable<CashTransferResultDto>>(cashTransfers);
    }

    public async ValueTask<IEnumerable<CashTransferResultDto>> RetrieveAllByCashRegisterIdAsync(long cashRegisterId)
    {
        var branchId = GetCurrentBranchId();
        var cashTransfers = await _repository.GetAll(t => t.CashRegisterId == cashRegisterId && !t.IsDeleted && t.BranchId.Equals(branchId))
            .ToListAsync();

        return _mapper.Map<IEnumerable<CashTransferResultDto>>(cashTransfers);
    }
}