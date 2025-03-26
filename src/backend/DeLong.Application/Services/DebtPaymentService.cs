using AutoMapper;
using DeLong.Application.Exceptions;
using DeLong.Application.Interfaces;
using DeLong.Domain.Entities;
using DeLong.Service.DTOs.DebtPayments;
using DeLong.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DeLong.Service.Services;

public class DebtPaymentService : AuditableService, IDebtPaymentService
{
    private readonly IMapper _mapper;
    private readonly IRepository<DebtPayment> _debtPaymentRepository;
    private readonly IRepository<Debt> _debtRepository;

    public DebtPaymentService(IRepository<DebtPayment> debtPaymentRepository, IMapper mapper, IRepository<Debt> debtRepository, IHttpContextAccessor httpContextAccessor)
        : base(httpContextAccessor)
    {
        _mapper = mapper;
        _debtPaymentRepository = debtPaymentRepository;
        _debtRepository = debtRepository;
    }

    public async ValueTask<DebtPaymentResultDto> AddAsync(DebtPaymentCreationDto dto)
    {
        var debt = await _debtRepository.GetAsync(d => d.Id == dto.DebtId && !d.IsDeleted)
            ?? throw new NotFoundException($"Debt not found with ID = {dto.DebtId}");

        if (dto.Amount > debt.RemainingAmount)
        {
            throw new Exception("To‘lov summasi qarz qoldig‘idan ko‘p bo‘lishi mumkin emas!");
        }

        var mappedDebtPayment = _mapper.Map<DebtPayment>(dto);
        SetCreatedFields(mappedDebtPayment); // Auditable maydonlarni qo‘shish

        await _debtPaymentRepository.CreateAsync(mappedDebtPayment);

        // Qarz qoldig‘ini yangilash
        debt.RemainingAmount -= dto.Amount;
        debt.IsSettled = debt.RemainingAmount <= 0; // IsSettled ni yangilash
        SetUpdatedFields(debt); // Debt uchun auditable maydonlarni yangilash
        _debtRepository.Update(debt);

        // Tranzaksiyalarni birlashtirish
        await _debtRepository.SaveChanges();

        return _mapper.Map<DebtPaymentResultDto>(mappedDebtPayment);
    }

    public async ValueTask<IEnumerable<DebtPaymentResultDto>> RetrieveByDebtIdAsync(long debtId)
    {
        var debtPayments = await _debtPaymentRepository.GetAll(dp => dp.DebtId == debtId && !dp.IsDeleted)
            .ToListAsync();
        return _mapper.Map<IEnumerable<DebtPaymentResultDto>>(debtPayments);
    }

    public async ValueTask<IEnumerable<DebtPaymentResultDto>> RetrieveAllAsync()
    {
        var debtPayments = await _debtPaymentRepository.GetAll(dp => !dp.IsDeleted)
            .ToListAsync();
        return _mapper.Map<IEnumerable<DebtPaymentResultDto>>(debtPayments);
    }
}