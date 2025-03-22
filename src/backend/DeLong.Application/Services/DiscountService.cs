using AutoMapper;
using DeLong.Application.Exceptions;
using DeLong.Application.Interfaces;
using DeLong.Domain.Entities;
using DeLong.Service.Interfaces;
using DeLong_Desktop.ApiService.DTOs.Discounts;
using Microsoft.EntityFrameworkCore;

namespace DeLong.Service.Services;

public class DiscountService : IDiscountService
{
    private readonly IRepository<Discount> discountRepository;
    private readonly IMapper mapper;

    public DiscountService(IRepository<Discount> discountRepository, IMapper mapper)
    {
        this.mapper = mapper;
        this.discountRepository = discountRepository;
    }

    public async ValueTask<DiscountResultDto> AddAsync(DiscountCreationDto dto)
    {
        var discount = mapper.Map<Discount>(dto);
        await discountRepository.CreateAsync(discount);
        await discountRepository.SaveChanges();
        return mapper.Map<DiscountResultDto>(discount);
    }

    public async ValueTask<DiscountResultDto> ModifyAsync(DiscountUpdateDto dto)
    {
        var discount = await discountRepository.GetAsync(u => u.Id.Equals(dto.Id));
        if (discount == null)
            throw new NotFoundException($"Bu Id={dto.Id} chegirma topilmadi.");

        var mappedDictount = mapper.Map(dto, discount);
        discountRepository.Update(mappedDictount);
        await discountRepository.SaveChanges();
        return mapper.Map<DiscountResultDto>(mappedDictount);
    }

    public async ValueTask<bool> RemoveAsync(long id)
    {
        Discount existDiscount = await this.discountRepository.GetAsync(u => u.Id.Equals(id))
            ?? throw new NotFoundException($"This Discount is not found with ID = {id}");

        this.discountRepository.Delete(existDiscount);
        await this.discountRepository.SaveChanges();
        return true;
    }

    public async ValueTask<DiscountResultDto> RetrieveByIdAsync(long id)
    {
        var discount = await discountRepository.GetAsync(u => u.Id.Equals(id));
        return discount == null ? null : mapper.Map<DiscountResultDto>(discount);
    }

    public async ValueTask<IEnumerable<DiscountResultDto>> RetrieveAllAsync()
    {
        var discounts = discountRepository.GetAll();
        return mapper.Map<IEnumerable<DiscountResultDto>>(discounts);
    }

    public async ValueTask<IEnumerable<DiscountResultDto>> RetrieveBySaleIdAsync(long saleId)
    {
        var discounts = await this.discountRepository.GetAll(d => d.SaleId == saleId).ToListAsync();
        return this.mapper.Map<IEnumerable<DiscountResultDto>>(discounts);
    }
}