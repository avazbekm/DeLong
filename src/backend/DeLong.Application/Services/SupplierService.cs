using AutoMapper;
using DeLong.Domain.Entities;
using DeLong.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using DeLong.Domain.Configurations;
using DeLong.Application.Exceptions;
using DeLong.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using DeLong.Application.DTOs.Suppliers;

namespace DeLong.Service.Services;

public class SupplierService : AuditableService, ISupplierService
{
    private readonly IMapper _mapper;
    private readonly IRepository<Supplier> _supplierRepository;

    public SupplierService(IRepository<Supplier> supplierRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(httpContextAccessor)
    {
        _mapper = mapper;
        _supplierRepository = supplierRepository;
    }

    public async ValueTask<SupplierResultDto> AddAsync(SupplierCreationDto dto)
    {
        var mappedSupplier = _mapper.Map<Supplier>(dto);
        SetCreatedFields(mappedSupplier);
        mappedSupplier.BranchId = GetCurrentBranchId();

        await _supplierRepository.CreateAsync(mappedSupplier);
        await _supplierRepository.SaveChanges();

        return _mapper.Map<SupplierResultDto>(mappedSupplier);
    }

    public async ValueTask<SupplierResultDto> ModifyAsync(SupplierUpdateDto dto)
    {
        var existSupplier = await _supplierRepository.GetAsync(s => s.Id.Equals(dto.Id) && !s.IsDeleted)
            ?? throw new NotFoundException($"Supplier not found with ID = {dto.Id}");

        _mapper.Map(dto, existSupplier);
        SetUpdatedFields(existSupplier);

        _supplierRepository.Update(existSupplier);
        await _supplierRepository.SaveChanges();

        return _mapper.Map<SupplierResultDto>(existSupplier);
    }

    public async ValueTask<bool> RemoveAsync(long id)
    {
        var existSupplier = await _supplierRepository.GetAsync(s => s.Id.Equals(id) && !s.IsDeleted)
            ?? throw new NotFoundException($"Supplier not found with ID = {id}");

        existSupplier.IsDeleted = true;
        SetUpdatedFields(existSupplier);

        _supplierRepository.Update(existSupplier);
        await _supplierRepository.SaveChanges();
        return true;
    }

    public async ValueTask<SupplierResultDto> RetrieveByIdAsync(long id)
    {
        var branchId = GetCurrentBranchId();
        var existSupplier = await _supplierRepository.GetAsync(s => s.Id.Equals(id) && !s.IsDeleted && s.BranchId.Equals(branchId))
            ?? throw new NotFoundException($"Supplier not found with ID = {id}");

        return _mapper.Map<SupplierResultDto>(existSupplier);
    }

    public async ValueTask<IEnumerable<SupplierResultDto>> RetrieveAllAsync()
    {
        var branchId = GetCurrentBranchId();
        var suppliers = await _supplierRepository.GetAll(s => !s.IsDeleted && s.BranchId.Equals(branchId)).ToListAsync();
        return _mapper.Map<IEnumerable<SupplierResultDto>>(suppliers);
    }

}
