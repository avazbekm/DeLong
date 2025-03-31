﻿using AutoMapper;
using DeLong.Application.DTOs.Products;
using DeLong.Application.Exceptions;
using DeLong.Application.Extensions;
using DeLong.Application.Interfaces;
using DeLong.Domain.Configurations;
using DeLong.Domain.Entities;
using DeLong.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DeLong.Service.Services;

public class ProductService : AuditableService, IProductService
{
    private readonly IMapper _mapper;
    private readonly IRepository<Product> _productRepository;

    public ProductService(IRepository<Product> productRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(httpContextAccessor)
    {
        _mapper = mapper;
        _productRepository = productRepository;
    }

    public async ValueTask<ProductResultDto> AddAsync(ProductCreationDto dto)
    {
        Product existProduct = await _productRepository.GetAsync(u => u.Name.Equals(dto.Name));
        if (existProduct is not null)
            throw new AlreadyExistException($"This Product is already exists with Name = {dto.Name}");

        var mappedProduct = _mapper.Map<Product>(dto);
        SetCreatedFields(mappedProduct); // Auditable maydonlarni qo‘shish
        mappedProduct.BranchId = GetCurrentBranchId();
        await _productRepository.CreateAsync(mappedProduct);
        await _productRepository.SaveChanges();

        var result = _mapper.Map<ProductResultDto>(mappedProduct);
        return result;
    }

    public async ValueTask<ProductResultDto> ModifyAsync(ProductUpdateDto dto)
    {
        Product existProduct = await _productRepository.GetAsync(u => u.Id.Equals(dto.Id))
            ?? throw new NotFoundException($"This Product is not found with ID = {dto.Id}");

        _mapper.Map(dto, existProduct);
        SetUpdatedFields(existProduct); // Auditable maydonlarni yangilash

        _productRepository.Update(existProduct);
        await _productRepository.SaveChanges();

        var result = _mapper.Map<ProductResultDto>(existProduct);
        return result;
    }

    public async ValueTask<bool> RemoveAsync(long id)
    {
        Product existProduct = await _productRepository.GetAsync(u => u.Id.Equals(id))
            ?? throw new NotFoundException($"This Product is not found with ID = {id}");

        existProduct.IsDeleted = true; // Soft delete uchun
        SetUpdatedFields(existProduct); // Auditable maydonlarni yangilash

        _productRepository.Update(existProduct); // Delete o‘rniga Update, chunki soft delete
        await _productRepository.SaveChanges();
        return true;
    }

    public async ValueTask<ProductResultDto> RetrieveByIdAsync(long id)
    {
        var branchId = GetCurrentBranchId();
        Product existProduct = await _productRepository.GetAsync(u => u.Id.Equals(id) && !u.IsDeleted && u.BranchId.Equals(branchId))
            ?? throw new NotFoundException($"This Product is not found with ID = {id}");

        var result = _mapper.Map<ProductResultDto>(existProduct);
        return result;
    }

    public async ValueTask<IEnumerable<ProductResultDto>> RetrieveAllAsync(PaginationParams @params, Filter filter, string search = null)
    {
        var products = await _productRepository.GetAll(p => !p.IsDeleted) // Faqat o‘chirilmaganlar
            .ToPaginate(@params)
            .OrderBy(filter)
            .ToListAsync();

        var result = products.Where(product => search == null || product.Name.Contains(search, StringComparison.OrdinalIgnoreCase));
        var mappedProducts = _mapper.Map<List<ProductResultDto>>(result);
        return mappedProducts;
    }

    public async ValueTask<IEnumerable<ProductResultDto>> RetrieveAllAsync()
    {
        var branchId = GetCurrentBranchId();
        var products = await _productRepository.GetAll(p => !p.IsDeleted && p.BranchId.Equals(branchId)) // Faqat o‘chirilmaganlar
            .ToListAsync();
        var result = _mapper.Map<IEnumerable<ProductResultDto>>(products);
        return result;
    }
}