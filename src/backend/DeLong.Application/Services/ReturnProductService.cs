using AutoMapper;
using DeLong.Application.Exceptions;
using DeLong.Application.Interfaces;
using DeLong.Domain.Entities;
using DeLong.Service.DTOs;
using DeLong.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DeLong.Service.Services;

public class ReturnProductService : AuditableService, IReturnProductService
{
    private readonly IRepository<ReturnProduct> _returnProductRepository;
    private readonly IRepository<Price> _priceRepository;
    private readonly IMapper _mapper;

    public ReturnProductService(IRepository<ReturnProduct> returnProductRepository, IRepository<Price> priceRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(httpContextAccessor)
    {
        _returnProductRepository = returnProductRepository;
        _priceRepository = priceRepository;
        _mapper = mapper;
    }

    public async ValueTask<ReturnProductResultDto> AddAsync(ReturnProductCreationDto dto)
    {
        using (var transaction = await _priceRepository.BeginTransactionAsync())
        {
            try
            {
                var branchId = GetCurrentBranchId();
                // Mahsulot narxini olish va sonini oshirish
                var price = await _priceRepository.GetAsync(p => p.ProductId == dto.ProductId && !p.IsDeleted && p.BranchId.Equals(branchId))
                    ?? throw new NotFoundException($"Mahsulot topilmadi (ProductId: {dto.ProductId})");

                price.Quantity += dto.Quantity;
                SetUpdatedFields(price); // Price uchun auditable maydonlarni yangilash
                price.BranchId = GetCurrentBranchId();
                _priceRepository.Update(price);

                // Qaytgan mahsulotni saqlash
                var returnProduct = _mapper.Map<ReturnProduct>(dto);
                SetCreatedFields(returnProduct); // Auditable maydonlarni qo‘shish
                returnProduct.BranchId = GetCurrentBranchId();
                await _returnProductRepository.CreateAsync(returnProduct);

                await _priceRepository.SaveChanges();
                transaction.Commit();

                return _mapper.Map<ReturnProductResultDto>(returnProduct);
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }
    }

    public async ValueTask<ReturnProductResultDto> ModifyAsync(ReturnProductUpdateDto dto)
    {
        using (var transaction = await _priceRepository.BeginTransactionAsync())
        {
            try
            {
                var branchId = GetCurrentBranchId();
                var returnProduct = await _returnProductRepository.GetAsync(rp => rp.Id == dto.Id && !rp.IsDeleted && rp.BranchId.Equals(branchId))
                    ?? throw new NotFoundException($"Qaytgan mahsulot topilmadi (Id: {dto.Id})");

                // Oldingi miqdor va yangi miqdor farqini hisoblash
                var oldQuantity = returnProduct.Quantity;
                var quantityDifference = dto.Quantity - oldQuantity;


                // Mahsulot sonini yangilash
                var price = await _priceRepository.GetAsync(p => p.ProductId == dto.ProductId && !p.IsDeleted && p.BranchId.Equals(branchId))
                    ?? throw new NotFoundException($"Mahsulot topilmadi (ProductId: {dto.ProductId})");

                price.Quantity += quantityDifference;
                SetUpdatedFields(price); // Price uchun auditable maydonlarni yangilash
                _priceRepository.Update(price);

                // Qaytgan mahsulotni yangilash
                _mapper.Map(dto, returnProduct);
                SetUpdatedFields(returnProduct); // Auditable maydonlarni yangilash
                _returnProductRepository.Update(returnProduct);

                await _priceRepository.SaveChanges();
                transaction.Commit();

                return _mapper.Map<ReturnProductResultDto>(returnProduct);
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }
    }

    public async ValueTask<bool> RemoveAsync(long id)
    {
        var returnProduct = await _returnProductRepository.GetAsync(rp => rp.Id == id && !rp.IsDeleted)
            ?? throw new NotFoundException($"Qaytgan mahsulot topilmadi (Id: {id})");

        returnProduct.IsDeleted = true; // Soft delete
        SetUpdatedFields(returnProduct); // Auditable maydonlarni yangilash
        _returnProductRepository.Update(returnProduct);
        await _returnProductRepository.SaveChanges();

        return true;
    }

    public async ValueTask<ReturnProductResultDto> RetrieveByIdAsync(long id)
    {
        var branchId = GetCurrentBranchId();
        var returnProduct = await _returnProductRepository.GetAsync(rp => rp.Id == id && !rp.IsDeleted && rp.BranchId.Equals(branchId))
            ?? throw new NotFoundException($"Qaytgan mahsulot topilmadi (Id: {id})");

        return _mapper.Map<ReturnProductResultDto>(returnProduct);
    }

    public async ValueTask<IEnumerable<ReturnProductResultDto>> RetrieveAllAsync()
    {
        var branchId = GetCurrentBranchId();
        var returnProducts = await _returnProductRepository.GetAll(rp => !rp.IsDeleted && rp.BranchId.Equals(branchId))
            .ToListAsync();
        return _mapper.Map<IEnumerable<ReturnProductResultDto>>(returnProducts);
    }

    public async ValueTask<IEnumerable<ReturnProductResultDto>> RetrieveBySaleIdAsync(long saleId)
    {
        var branchId = GetCurrentBranchId();
        var returnProducts = await _returnProductRepository.GetAll(rp => rp.SaleId == saleId && !rp.IsDeleted && rp.BranchId.Equals(branchId))
            .ToListAsync();
        return _mapper.Map<IEnumerable<ReturnProductResultDto>>(returnProducts);
    }

    public async ValueTask<IEnumerable<ReturnProductResultDto>> RetrieveByProductIdAsync(long productId)
    {
        var branchId = GetCurrentBranchId();
        var returnProducts = await _returnProductRepository.GetAll(rp => rp.ProductId == productId && !rp.IsDeleted && rp.BranchId.Equals(branchId))
            .ToListAsync();
        return _mapper.Map<IEnumerable<ReturnProductResultDto>>(returnProducts);
    }
}