using AutoMapper;
using DeLong.Domain.Entities;
using DeLong.Service.Interfaces;
using DeLong.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using DeLong.Service.DTOs.DebtPayments;
using DeLong.Application.Exceptions;

namespace DeLong.Service.Services;

public class DebtPaymentService : IDebtPaymentService
{
    private readonly IMapper mapper;
    private readonly IRepository<DebtPayment> debtPaymentRepository;
    private readonly IRepository<Debt> debtRepository;

    public DebtPaymentService(IRepository<DebtPayment> debtPaymentRepository, IMapper mapper, IRepository<Debt> debtRepository)
    {
        this.mapper = mapper;
        this.debtPaymentRepository = debtPaymentRepository;
        this.debtRepository = debtRepository;
    }

    public async ValueTask<DebtPaymentResultDto> AddAsync(DebtPaymentCreationDto dto)
    {
        var debt = await this.debtRepository.GetAsync(d => d.Id == dto.DebtId)
            ?? throw new NotFoundException($"Debt not found with ID = {dto.DebtId}");

        if (dto.Amount > debt.RemainingAmount)
        {
            throw new Exception("To‘lov summasi qarz qoldig‘idan ko‘p bo‘lishi mumkin emas!");
        }

        var mappedDebtPayment = this.mapper.Map<DebtPayment>(dto);

        await this.debtPaymentRepository.CreateAsync(mappedDebtPayment);

        // Qarz qoldig‘ini yangilash
        debt.RemainingAmount -= dto.Amount;
        debt.IsSettled = debt.RemainingAmount <= 0; // IsSettled ni yangilash
        this.debtRepository.Update(debt);

        // Tranzaksiyalarni birlashtirish
        await this.debtRepository.SaveChanges();

        return this.mapper.Map<DebtPaymentResultDto>(mappedDebtPayment);
    }

    public async ValueTask<IEnumerable<DebtPaymentResultDto>> RetrieveByDebtIdAsync(long debtId)
    {
        var debtPayments = await this.debtPaymentRepository.GetAll(dp => dp.DebtId == debtId)
            .ToListAsync();
        return this.mapper.Map<IEnumerable<DebtPaymentResultDto>>(debtPayments);
    }

    public async ValueTask<IEnumerable<DebtPaymentResultDto>> RetrieveAllAsync()
    {
        var debtPayments = await this.debtPaymentRepository.GetAll()
            .ToListAsync();
        return this.mapper.Map<IEnumerable<DebtPaymentResultDto>>(debtPayments);
    }
}