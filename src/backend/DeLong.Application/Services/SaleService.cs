using AutoMapper;
using DeLong.Application.Exceptions;
using DeLong.Application.Interfaces;
using DeLong.Domain.Entities;
using DeLong.Service.DTOs.Sale;
using DeLong.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DeLong.Service.Services;

public class SaleService : AuditableService, ISaleService
{
    private readonly IMapper _mapper;
    private readonly IRepository<Sale> _saleRepository;

    public SaleService(IRepository<Sale> saleRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(httpContextAccessor)
    {
        _mapper = mapper;
        _saleRepository = saleRepository;
    }

    public async ValueTask<SaleResultDto> AddAsync(SaleCreationDto dto)
    {
        var newSale = _mapper.Map<Sale>(dto);
        SetCreatedFields(newSale); // Auditable maydonlarni qo‘shish (CreatedBy, CreatedAt)
        newSale.BranchId = GetCurrentBranchId();

        await _saleRepository.CreateAsync(newSale);
        await _saleRepository.SaveChanges();

        var result = _mapper.Map<SaleResultDto>(newSale);
        result.PaidAmount = newSale.PaidAmount; // Computed property
        result.RemainingAmount = newSale.RemainingAmount;
        result.Status = newSale.Status.ToString(); // Enum’dan string’ga
        return result;
    }

    public async ValueTask<SaleResultDto> RetrieveByIdAsync(long id)
    {
        var branchId = GetCurrentBranchId();
        var sale = await _saleRepository.GetAsync(s => s.Id == id && !s.IsDeleted && s.BranchId.Equals(branchId),
            includes: new[] { "Payments", "Debts", "Customer", "User" }) // Bog‘liq ma’lumotlarni include qilish
            ?? throw new NotFoundException($"Sale not found with ID = {id}");

        var result = _mapper.Map<SaleResultDto>(sale);
        result.PaidAmount = sale.PaidAmount; // Computed property
        result.RemainingAmount = sale.RemainingAmount;
        result.Status = sale.Status.ToString(); // Enum’dan string’ga
        result.CustomerName = sale.Customer?.CompanyName ?? string.Empty; // CustomerName qo‘shish
        result.UserName = sale.User != null ? $"{sale.User.FirstName} {sale.User.LastName}".Trim() : string.Empty; // UserName hosil qilish
        return result;
    }

    public async ValueTask<IEnumerable<SaleResultDto>> RetrieveAllAsync()
    {
        var branchId = GetCurrentBranchId();

        var sales = await _saleRepository.GetAll(s => !s.IsDeleted && s.BranchId.Equals(branchId),
            includes: new[] { "Payments", "Debts", "Customer", "User" }) // Bog‘liq ma’lumotlarni include qilish
            .ToListAsync();

        var result = _mapper.Map<IEnumerable<SaleResultDto>>(sales);
        foreach (var saleDto in result)
        {
            var sale = sales.First(s => s.Id == saleDto.Id);
            saleDto.PaidAmount = sale.PaidAmount; // Computed property
            saleDto.RemainingAmount = sale.RemainingAmount;
            saleDto.Status = sale.Status.ToString(); // Enum’dan string’ga
            saleDto.CustomerName = sale.Customer?.CompanyName ?? string.Empty; // CustomerName qo‘shish
            saleDto.UserName = sale.User != null ? $"{sale.User.FirstName} {sale.User.LastName}".Trim() : string.Empty; // UserName hosil qilish
        }
        return result;
    }
}