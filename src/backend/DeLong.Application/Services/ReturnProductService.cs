using AutoMapper;
using DeLong.Service.DTOs;
using DeLong.Domain.Entities;
using DeLong.Service.Interfaces;
using DeLong.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using DeLong.Application.Exceptions;

namespace DeLong.Service.Services;

public class ReturnProductService : IReturnProductService
{
    private readonly IRepository<ReturnProduct> returnProductRepository;
    private readonly IRepository<Price> priceRepository;
    private readonly IMapper mapper;

    public ReturnProductService(IRepository<ReturnProduct> returnProductRepository, IRepository<Price> priceRepository, IMapper mapper)
    {
        this.returnProductRepository = returnProductRepository;
        this.priceRepository = priceRepository;
        this.mapper = mapper;
    }

    public async ValueTask<ReturnProductResultDto> AddAsync(ReturnProductCreationDto dto)
    {
        using (var transaction = await priceRepository.BeginTransactionAsync())
        {
            try
            {
                // Mahsulot narxini olish va sonini oshirish
                var price = await priceRepository.GetAsync(p => p.ProductId == dto.ProductId);
                if (price == null)
                {
                    throw new NotFoundException($"Mahsulot topilmadi (ProductId: {dto.ProductId})");
                }

                price.Quantity += dto.Quantity;
                priceRepository.Update(price);

                // Qaytgan mahsulotni saqlash
                var returnProduct = mapper.Map<ReturnProduct>(dto);
                await returnProductRepository.CreateAsync(returnProduct);

                await priceRepository.SaveChanges();
                transaction.Commit();

                return mapper.Map<ReturnProductResultDto>(returnProduct);
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
        using (var transaction = await priceRepository.BeginTransactionAsync())
        {
            try
            {
                var returnProduct = await returnProductRepository.GetAsync(rp => rp.Id == dto.Id);
                if (returnProduct == null)
                {
                    throw new NotFoundException($"Qaytgan mahsulot topilmadi (Id: {dto.Id})");
                }

                // Oldingi miqdor va yangi miqdor farqini hisoblash
                var oldQuantity = returnProduct.Quantity;
                var quantityDifference = dto.Quantity - oldQuantity;

                // Mahsulot sonini yangilash
                var price = await priceRepository.GetAsync(p => p.ProductId == dto.ProductId);
                if (price == null)
                {
                    throw new NotFoundException($"Mahsulot topilmadi (ProductId: {dto.ProductId})");
                }

                price.Quantity += quantityDifference;
                priceRepository.Update(price);

                // Qaytgan mahsulotni yangilash
                mapper.Map(dto, returnProduct);
                returnProductRepository.Update(returnProduct);

                await priceRepository.SaveChanges();
                transaction.Commit();

                return mapper.Map<ReturnProductResultDto>(returnProduct);
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
        var returnProduct = await returnProductRepository.GetAsync(rp => rp.Id == id);
        if (returnProduct == null)
        {
            throw new NotFoundException($"Qaytgan mahsulot topilmadi (Id: {id})");
        }

        // Soft delete
        returnProductRepository.Delete(returnProduct);
        await returnProductRepository.SaveChanges();

        return true;
    }

    public async ValueTask<ReturnProductResultDto> RetrieveByIdAsync(long id)
    {
        var returnProduct = await returnProductRepository.GetAsync(rp => rp.Id == id && !rp.IsDeleted);
        if (returnProduct == null)
        {
            throw new NotFoundException($"Qaytgan mahsulot topilmadi (Id: {id})");
        }

        return mapper.Map<ReturnProductResultDto>(returnProduct);
    }

    public async ValueTask<IEnumerable<ReturnProductResultDto>> RetrieveAllAsync()
    {
        var returnProducts = await returnProductRepository.GetAll(rp => !rp.IsDeleted).ToListAsync();
        return mapper.Map<IEnumerable<ReturnProductResultDto>>(returnProducts);
    }

    public async ValueTask<IEnumerable<ReturnProductResultDto>> RetrieveBySaleIdAsync(long saleId)
    {
        var returnProducts = await returnProductRepository.GetAll(rp => rp.SaleId == saleId && !rp.IsDeleted).ToListAsync();
        return mapper.Map<IEnumerable<ReturnProductResultDto>>(returnProducts);
    }

    public async ValueTask<IEnumerable<ReturnProductResultDto>> RetrieveByProductIdAsync(long productId)
    {
        var returnProducts = await returnProductRepository.GetAll(rp => rp.ProductId == productId && !rp.IsDeleted).ToListAsync();
        return mapper.Map<IEnumerable<ReturnProductResultDto>>(returnProducts);
    }
}