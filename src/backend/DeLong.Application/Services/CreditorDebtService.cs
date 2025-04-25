using AutoMapper;
using DeLong.Domain.Entities;
using DeLong.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using DeLong.Application.Exceptions;
using DeLong.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using DeLong.Service.DTOs.CreditorDebts;

namespace DeLong.Service.Services;

public class CreditorDebtService : AuditableService, ICreditorDebtService
{
    private readonly IMapper _mapper;
    private readonly IRepository<CreditorDebt> _creditorDebtRepository;

    public CreditorDebtService(
        IRepository<CreditorDebt> creditorDebtRepository,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor)
        : base(httpContextAccessor)
    {
        _mapper = mapper;
        _creditorDebtRepository = creditorDebtRepository;
    }

    public async ValueTask<CreditorDebtResultDto> AddAsync(CreditorDebtCreationDto dto)
    {
        // Ta'minotchi va filialning mavjudligini tekshirish mumkin
        var existDebt = await _creditorDebtRepository.GetAsync(d =>
            d.SupplierId == dto.SupplierId &&
            d.Date == dto.Date &&
            d.BranchId == dto.BranchId &&
            !d.IsDeleted);

        if (existDebt != null)
            throw new AlreadyExistException($"Bu ta'minotchi uchun qarzdorlik allaqachon mavjud (SupplierId: {dto.SupplierId}, Date: {dto.Date})");

        var mappedDebt = _mapper.Map<CreditorDebt>(dto);
        SetCreatedFields(mappedDebt); // Auditable maydonlarni qo‘shish
        mappedDebt.BranchId = GetCurrentBranchId();
        await _creditorDebtRepository.CreateAsync(mappedDebt);
        await _creditorDebtRepository.SaveChanges();

        return _mapper.Map<CreditorDebtResultDto>(mappedDebt);
    }

    public async ValueTask<CreditorDebtResultDto> ModifyAsync(CreditorDebtUpdateDto dto)
    {
        var branchId = GetCurrentBranchId();
        var existDebt = await _creditorDebtRepository.GetAsync(d =>
            d.Id == dto.Id && !d.IsDeleted && d.BranchId == branchId)
            ?? throw new NotFoundException($"Bu qarzdorlik topilmadi (ID: {dto.Id})");

        _mapper.Map(dto, existDebt);
        SetUpdatedFields(existDebt); // Auditable maydonlarni yangilash
        existDebt.BranchId = branchId;

        _creditorDebtRepository.Update(existDebt);
        await _creditorDebtRepository.SaveChanges();

        return _mapper.Map<CreditorDebtResultDto>(existDebt);
    }

    public async ValueTask<bool> RemoveAsync(long id)
    {
        var branchId = GetCurrentBranchId();
        var existDebt = await _creditorDebtRepository.GetAsync(d =>
            d.Id == id && !d.IsDeleted && d.BranchId == branchId)
            ?? throw new NotFoundException($"Bu qarzdorlik topilmadi (ID: {id})");

        existDebt.IsDeleted = true; // Soft delete
        SetUpdatedFields(existDebt); // Auditable maydonlarni yangilash

        _creditorDebtRepository.Update(existDebt);
        await _creditorDebtRepository.SaveChanges();
        return true;
    }

    public async ValueTask<CreditorDebtResultDto> RetrieveByIdAsync(long id)
    {
        var branchId = GetCurrentBranchId();
        var existDebt = await _creditorDebtRepository.GetAsync(d =>
            d.Id == id && !d.IsDeleted && d.BranchId == branchId,
            includes: new[] { "Supplier", "CreditorDebtPayments" })
            ?? throw new NotFoundException($"Bu qarzdorlik topilmadi (ID: {id})");

        return _mapper.Map<CreditorDebtResultDto>(existDebt);
    }

    public async ValueTask<IEnumerable<CreditorDebtResultDto>> RetrieveAllAsync()
    {
        var branchId = GetCurrentBranchId();
        var debts = await _creditorDebtRepository.GetAll(d =>
            !d.IsDeleted && d.BranchId == branchId,
            includes: new[] { "Supplier", "CreditorDebtPayments" })
            .ToListAsync();

        return _mapper.Map<List<CreditorDebtResultDto>>(debts);
    }

}