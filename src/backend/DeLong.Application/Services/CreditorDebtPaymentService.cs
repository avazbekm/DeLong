using AutoMapper;
using DeLong.Domain.Entities;
using DeLong.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using DeLong.Application.Exceptions;
using DeLong.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using DeLong.Service.DTOs.CreditorDebtPayments;
using System.ComponentModel.DataAnnotations;

namespace DeLong.Service.Services;

public class CreditorDebtPaymentService : AuditableService, ICreditorDebtPaymentService
{
    private readonly IMapper _mapper;
    private readonly IRepository<CreditorDebtPayment> _paymentRepository;
    private readonly IRepository<CreditorDebt> _creditorDebtRepository;

    public CreditorDebtPaymentService(
        IRepository<CreditorDebtPayment> paymentRepository,
        IRepository<CreditorDebt> creditorDebtRepository,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor)
        : base(httpContextAccessor)
    {
        _mapper = mapper;
        _paymentRepository = paymentRepository;
        _creditorDebtRepository = creditorDebtRepository;
    }

    public async ValueTask<CreditorDebtPaymentResultDto> AddAsync(CreditorDebtPaymentCreationDto dto)
    {
        var branchId = GetCurrentBranchId();
        var creditorDebt = await _creditorDebtRepository.GetAsync(d =>
            d.Id == dto.CreditorDebtId && !d.IsDeleted && d.BranchId == branchId)
            ?? throw new NotFoundException($"Qarzdorlik topilmadi (ID: {dto.CreditorDebtId})");

        if (dto.Amount > creditorDebt.RemainingAmount)
            throw new ValidationException($"To'lov summasi qoldiq qarzdan oshib ketdi (Qoldiq: {creditorDebt.RemainingAmount})");

        var mappedPayment = _mapper.Map<CreditorDebtPayment>(dto);
        SetCreatedFields(mappedPayment); // Auditable maydonlarni qo‘shish
        mappedPayment.BranchId = branchId;

        await _paymentRepository.CreateAsync(mappedPayment);

        // Qarzdorlik qoldig'ini yangilash
        creditorDebt.RemainingAmount -= dto.Amount;
        creditorDebt.IsSettled = creditorDebt.RemainingAmount <= 0;
        SetUpdatedFields(creditorDebt);
        _creditorDebtRepository.Update(creditorDebt);

        await _paymentRepository.SaveChanges();

        return _mapper.Map<CreditorDebtPaymentResultDto>(mappedPayment);
    }

    public async ValueTask<CreditorDebtPaymentResultDto> ModifyAsync(CreditorDebtPaymentUpdateDto dto)
    {
        var branchId = GetCurrentBranchId();
        var existPayment = await _paymentRepository.GetAsync(p =>
            p.Id == dto.Id && !p.IsDeleted && p.BranchId == branchId,
            includes: new[] { "CreditorDebt" })
            ?? throw new NotFoundException($"Bu to'lov topilmadi (ID: {dto.Id})");

        var oldAmount = existPayment.Amount; // Eski to'lov summasini saqlash
        _mapper.Map(dto, existPayment);
        SetUpdatedFields(existPayment); // Auditable maydonlarni yangilash
        existPayment.BranchId = branchId;

        // Qarzdorlik qoldig'ini yangilash
        var creditorDebt = existPayment.CreditorDebt;
        if (dto.Amount.HasValue && dto.Amount.Value != oldAmount)
        {
            creditorDebt.RemainingAmount += oldAmount; // Eski to'lovni qaytarish
            creditorDebt.RemainingAmount -= dto.Amount.Value; // Yangi to'lovni ayirish
            creditorDebt.IsSettled = creditorDebt.RemainingAmount <= 0;
            SetUpdatedFields(creditorDebt);
            _creditorDebtRepository.Update(creditorDebt);
        }

        _paymentRepository.Update(existPayment);
        await _paymentRepository.SaveChanges();

        return _mapper.Map<CreditorDebtPaymentResultDto>(existPayment);
    }

    public async ValueTask<bool> RemoveAsync(long id)
    {
        var branchId = GetCurrentBranchId();
        var existPayment = await _paymentRepository.GetAsync(p =>
            p.Id == id && !p.IsDeleted && p.BranchId == branchId,
            includes: new[] { "CreditorDebt" })
            ?? throw new NotFoundException($"Bu to'lov topilmadi (ID: {id})");

        existPayment.IsDeleted = true; // Soft delete
        SetUpdatedFields(existPayment); // Auditable maydonlarni yangilash

        // Qarzdorlik qoldig'ini yangilash
        var creditorDebt = existPayment.CreditorDebt;
        creditorDebt.RemainingAmount += existPayment.Amount; // To'lov summasini qaytarish
        creditorDebt.IsSettled = creditorDebt.RemainingAmount <= 0;
        SetUpdatedFields(creditorDebt);
        _creditorDebtRepository.Update(creditorDebt);

        _paymentRepository.Update(existPayment);
        await _paymentRepository.SaveChanges();
        return true;
    }

    public async ValueTask<CreditorDebtPaymentResultDto> RetrieveByIdAsync(long id)
    {
        var branchId = GetCurrentBranchId();
        var existPayment = await _paymentRepository.GetAsync(p =>
            p.Id == id && !p.IsDeleted && p.BranchId == branchId,
            includes: new[] { "CreditorDebt" })
            ?? throw new NotFoundException($"Bu to'lov topilmadi (ID: {id})");

        return _mapper.Map<CreditorDebtPaymentResultDto>(existPayment);
    }

    public async ValueTask<IEnumerable<CreditorDebtPaymentResultDto>> RetrieveAllAsync()
    {
        var branchId = GetCurrentBranchId();
        var payments = await _paymentRepository.GetAll(p =>
            !p.IsDeleted && p.BranchId == branchId,
            includes: new[] { "CreditorDebt" })
            .ToListAsync();

        return _mapper.Map<List<CreditorDebtPaymentResultDto>>(payments);
    }
}