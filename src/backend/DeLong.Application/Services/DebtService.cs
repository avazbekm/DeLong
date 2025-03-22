using AutoMapper;
using DeLong.Application.Exceptions;
using DeLong.Application.Interfaces;
using DeLong.Domain.Entities;
using DeLong.Service.DTOs.Debts;
using DeLong.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeLong.Service.Services;

public class DebtService : IDebtService
{
    private readonly IRepository<Debt> debtRepository;
    private readonly IMapper mapper;

    public DebtService(IRepository<Debt> debtRepository, IMapper mapper)
    {
        this.debtRepository = debtRepository;
        this.mapper = mapper;
    }

    public async ValueTask<DebtResultDto> AddAsync(DebtCreationDto dto)
    {
        var newDebt = this.mapper.Map<Debt>(dto);
        await this.debtRepository.CreateAsync(newDebt);
        await this.debtRepository.SaveChanges();
        return this.mapper.Map<DebtResultDto>(newDebt);
    }

    public async ValueTask<DebtResultDto> ModifyAsync(DebtUpdateDto dto)
    {
        var existDebt = await this.debtRepository.GetAsync(d => d.Id == dto.Id)
            ?? throw new NotFoundException($"Debt not found with ID = {dto.Id}");

        this.mapper.Map(dto, existDebt);
        this.debtRepository.Update(existDebt);
        await this.debtRepository.SaveChanges();
        return this.mapper.Map<DebtResultDto>(existDebt);
    }

    public async ValueTask<bool> RemoveAsync(long id)
    {
        var existDebt = await this.debtRepository.GetAsync(d => d.Id == id)
            ?? throw new NotFoundException($"Debt not found with ID = {id}");

        this.debtRepository.Delete(existDebt);
        await this.debtRepository.SaveChanges();
        return true;
    }

    public async ValueTask<DebtResultDto> RetrieveByIdAsync(long id)
    {
        var existDebt = await this.debtRepository.GetAsync(d => d.Id == id)
            ?? throw new NotFoundException($"Debt not found with ID = {id}");

        return this.mapper.Map<DebtResultDto>(existDebt);
    }

    public async ValueTask<IEnumerable<DebtResultDto>> RetrieveAllAsync()
    {
        var debts = await this.debtRepository.GetAll()
            .Where(d => !d.IsSettled) // Faqat to‘lanmagan qarzlarni qaytaramiz
            .ToListAsync();
        return this.mapper.Map<IEnumerable<DebtResultDto>>(debts);
    }

    public async ValueTask<IEnumerable<DebtResultDto>> RetrieveBySaleIdAsync(long saleId)
    {
        var debts = await this.debtRepository.GetAll(d => d.SaleId == saleId && !d.IsSettled)
            .ToListAsync();
        return this.mapper.Map<IEnumerable<DebtResultDto>>(debts);
    }

    public async ValueTask<Dictionary<string, List<DebtResultDto>>> RetrieveAllGroupedByCustomerAsync()
    {
        var debts = await this.debtRepository.GetAll()
            .Where(d => !d.IsSettled) // Faqat to‘lanmagan qarzlarni olamiz
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
                group => this.mapper.Map<List<DebtResultDto>>(group.ToList())
            );

        return groupedDebts;
    }
}