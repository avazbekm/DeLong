using AutoMapper;
using DeLong.Application.Exceptions;
using DeLong.Application.Interfaces;
using DeLong.Domain.Entities;
using DeLong.Service.Interfaces;
using DeLong_Desktop.ApiService.DTOs.Discounts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DeLong.Service.Services;

public class DiscountService : AuditableService, IDiscountService
{
    private readonly IRepository<Discount> _discountRepository;
    private readonly IMapper _mapper;

    public DiscountService(IRepository<Discount> discountRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(httpContextAccessor)
    {
        _discountRepository = discountRepository;
        _mapper = mapper;
    }

    public async ValueTask<DiscountResultDto> AddAsync(DiscountCreationDto dto)
    {
        var discount = _mapper.Map<Discount>(dto);
        SetCreatedFields(discount); // Auditable maydonlarni qo‘shish
        discount.BranchId = GetCurrentBranchId();
        await _discountRepository.CreateAsync(discount);
        await _discountRepository.SaveChanges();
        return _mapper.Map<DiscountResultDto>(discount);
    }

    public async ValueTask<DiscountResultDto> ModifyAsync(DiscountUpdateDto dto)
    {
        var discount = await _discountRepository.GetAsync(u => u.Id.Equals(dto.Id) && !u.IsDeleted)
            ?? throw new NotFoundException($"Bu Id={dto.Id} chegirma topilmadi.");

        _mapper.Map(dto, discount);
        SetUpdatedFields(discount); // Auditable maydonlarni yangilash

        _discountRepository.Update(discount);
        await _discountRepository.SaveChanges();
        return _mapper.Map<DiscountResultDto>(discount);
    }

    public async ValueTask<bool> RemoveAsync(long id)
    {
        var existDiscount = await _discountRepository.GetAsync(u => u.Id.Equals(id) && !u.IsDeleted)
            ?? throw new NotFoundException($"This Discount is not found with ID = {id}");

        existDiscount.IsDeleted = true; // Soft delete
        SetUpdatedFields(existDiscount); // Auditable maydonlarni yangilash

        _discountRepository.Update(existDiscount);
        await _discountRepository.SaveChanges();
        return true;
    }

    public async ValueTask<DiscountResultDto> RetrieveByIdAsync(long id)
    {
        var branchId = GetCurrentBranchId();
        var discount = await _discountRepository.GetAsync(u => u.Id.Equals(id) && !u.IsDeleted && u.BranchId.Equals(branchId))
            ?? throw new NotFoundException($"This Discount is not found with ID = {id}");

        return _mapper.Map<DiscountResultDto>(discount);
    }

    public async ValueTask<IEnumerable<DiscountResultDto>> RetrieveAllAsync()
    {
        var branchId = GetCurrentBranchId();

        var discounts = await _discountRepository.GetAll(u => !u.IsDeleted && u.BranchId.Equals(branchId))
            .ToListAsync();
        return _mapper.Map<IEnumerable<DiscountResultDto>>(discounts);
    }

    public async ValueTask<IEnumerable<DiscountResultDto>> RetrieveBySaleIdAsync(long saleId)
    {
        var branchId = GetCurrentBranchId();
        var discounts = await _discountRepository.GetAll(d => d.SaleId == saleId && !d.IsDeleted && d.BranchId.Equals(branchId))
            .ToListAsync();
        return _mapper.Map<IEnumerable<DiscountResultDto>>(discounts);
    }
}