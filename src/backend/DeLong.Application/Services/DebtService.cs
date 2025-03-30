using AutoMapper;
using DeLong.Application.Exceptions;
using DeLong.Application.Interfaces;
using DeLong.Domain.Entities;
using DeLong.Service.DTOs.Debts;
using DeLong.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DeLong.Service.Services;

public class DebtService : AuditableService, IDebtService
{
    private readonly IRepository<Debt> _debtRepository;
    private readonly IMapper _mapper;

    public DebtService(IRepository<Debt> debtRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(httpContextAccessor)
    {
        _debtRepository = debtRepository;
        _mapper = mapper;
    }

    public async ValueTask<DebtResultDto> AddAsync(DebtCreationDto dto)
    {
        var newDebt = _mapper.Map<Debt>(dto);
        newDebt.IsSettled = false; // Yangi qarz default holatda to‘lanmagan
        SetCreatedFields(newDebt); // Auditable maydonlarni qo‘shish
        newDebt.BranchId = GetCurrentBranchId();
        await _debtRepository.CreateAsync(newDebt);
        await _debtRepository.SaveChanges();
        return _mapper.Map<DebtResultDto>(newDebt);
    }

    public async ValueTask<DebtResultDto> ModifyAsync(DebtUpdateDto dto)
    {
        var existDebt = await _debtRepository.GetAsync(d => d.Id == dto.Id && !d.IsDeleted)
            ?? throw new NotFoundException($"Debt not found with ID = {dto.Id}");

        _mapper.Map(dto, existDebt);
        SetUpdatedFields(existDebt); // Auditable maydonlarni yangilash

        _debtRepository.Update(existDebt);
        await _debtRepository.SaveChanges();
        return _mapper.Map<DebtResultDto>(existDebt);
    }

    public async ValueTask<bool> RemoveAsync(long id)
    {
        var existDebt = await _debtRepository.GetAsync(d => d.Id == id && !d.IsDeleted)
            ?? throw new NotFoundException($"Debt not found with ID = {id}");

        existDebt.IsDeleted = true; // Soft delete
        SetUpdatedFields(existDebt); // Auditable maydonlarni yangilash

        _debtRepository.Update(existDebt);
        await _debtRepository.SaveChanges();
        return true;
    }

    public async ValueTask<DebtResultDto> RetrieveByIdAsync(long id)
    {
        var branchId = GetCurrentBranchId();
        var existDebt = await _debtRepository.GetAsync(d => d.Id == id && !d.IsDeleted && d.BranchId.Equals(branchId))
            ?? throw new NotFoundException($"Debt not found with ID = {id}");

        return _mapper.Map<DebtResultDto>(existDebt);
    }

    public async ValueTask<IEnumerable<DebtResultDto>> RetrieveAllAsync()
    {
        var branchId = GetCurrentBranchId();
        var debts = await _debtRepository.GetAll(d => !d.IsDeleted && !d.IsSettled && d.BranchId.Equals(branchId)) // Faqat to‘lanmagan va o‘chirilmagan qarzlar
            .ToListAsync();
        return _mapper.Map<IEnumerable<DebtResultDto>>(debts);
    }

    public async ValueTask<IEnumerable<DebtResultDto>> RetrieveBySaleIdAsync(long saleId)
    {
        var branchId = GetCurrentBranchId();
        var debts = await _debtRepository.GetAll(d => d.SaleId == saleId && !d.IsSettled && !d.IsDeleted && d.BranchId.Equals(branchId))
            .ToListAsync();
        return _mapper.Map<IEnumerable<DebtResultDto>>(debts);
    }

    public async ValueTask<Dictionary<string, List<DebtResultDto>>> RetrieveAllGroupedByCustomerAsync()
    {
        var branchId = GetCurrentBranchId();
        var debts = await _debtRepository.GetAll(d => !d.IsSettled && !d.IsDeleted && d.BranchId.Equals(branchId)) // Faqat to‘lanmagan va o‘chirilmagan qarzlar
            .Include(d => d.Sale)
            .ThenInclude(s => s.User)
            .Include(d => d.Sale)
            .ThenInclude(s => s.Customer)
            .ToListAsync();

        var groupedDebts = debts
            .GroupBy(debt =>
            {
                if (debt.Sale?.UserId.HasValue == true && debt.Sale.User != null)
                    return $"{debt.Sale.User.FirstName} {debt.Sale.User.LastName}";
                else if (debt.Sale?.CustomerId.HasValue == true && debt.Sale.Customer != null)
                    return debt.Sale.Customer.Name;
                return "Noma'lum";
            })
            .Where(group => group.Any(debt => debt.RemainingAmount > 0))
            .ToDictionary(
                group => group.Key,
                group => _mapper.Map<List<DebtResultDto>>(group.ToList())
            );

        return groupedDebts;
    }
}