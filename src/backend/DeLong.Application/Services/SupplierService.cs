using AutoMapper;
using DeLong.Domain.Entities;
using DeLong.Service.Interfaces;
using DeLong.Domain.Configurations;
using Microsoft.EntityFrameworkCore;
using DeLong.Application.Exceptions;
using DeLong.Application.Extensions;
using DeLong.Application.Interfaces;
using DeLong.Application.DTOs.Suppliers;

namespace DeLong.Service.Services;

public class SupplierService:ISupplierService
{
    private readonly IMapper mapper;
    private readonly IRepository<Supplier> supplierRepository;
    public SupplierService(IRepository<Supplier> supplierRepository, IMapper mapper)
    {
        this.mapper = mapper;
        this.supplierRepository = supplierRepository;
    }

    public async ValueTask<SupplierResultDto> AddAsync(SupplierCreationDto dto)
    {
        Supplier existSuppliers = await this.supplierRepository.GetAsync(u => u.Name.Equals(dto.Name));
        if (existSuppliers is not null)
            throw new AlreadyExistException($"This Supplier is already exists with Name = {dto.Name}");

        var mappedSuppliers = this.mapper.Map<Supplier>(dto);
        await this.supplierRepository.CreateAsync(mappedSuppliers);
        await this.supplierRepository.SaveChanges();

        var result = this.mapper.Map<SupplierResultDto>(mappedSuppliers);
        return result;
    }

    public async ValueTask<SupplierResultDto> ModifyAsync(SupplierUpdateDto dto)
    {
        Supplier existSuppliers = await this.supplierRepository.GetAsync(u => u.Id.Equals(dto.Id))
            ?? throw new NotFoundException($"This Supplier is not found with ID = {dto.Id}");

        this.mapper.Map(dto, existSuppliers);
        this.supplierRepository.Update(existSuppliers);
        await this.supplierRepository.SaveChanges();

        var result = this.mapper.Map<SupplierResultDto>(existSuppliers);
        return result;
    }

    public async ValueTask<bool> RemoveAsync(long id)
    {
        Supplier existSuppliers = await this.supplierRepository.GetAsync(u => u.Id.Equals(id))
            ?? throw new NotFoundException($"This Supplier is not found with ID = {id}");

        this.supplierRepository.Delete(existSuppliers);
        await this.supplierRepository.SaveChanges();
        return true;
    }

    public async ValueTask<SupplierResultDto> RetrieveByIdAsync(long id)
    {
        Supplier existSuppliers = await this.supplierRepository.GetAsync(u => u.Id.Equals(id))
            ?? throw new NotFoundException($"This Supplier is not found with ID = {id}");

        var result = this.mapper.Map<SupplierResultDto>(existSuppliers);
        return result;
    }

    public async ValueTask<IEnumerable<SupplierResultDto>> RetrieveAllAsync(PaginationParams @params, Filter filter, string search = null)
    {
        var suppliers = await this.supplierRepository.GetAll()
            .ToPaginate(@params)
            .OrderBy(filter)
            .ToListAsync();

        var result = suppliers.Where(supplier => supplier.Id.ToString().Contains(search, StringComparison.OrdinalIgnoreCase));
        var mappedSuppliers = this.mapper.Map<List<SupplierResultDto>>(result);
        return mappedSuppliers;
    }

    public async ValueTask<IEnumerable<SupplierResultDto>> RetrieveAllAsync()
    {
        var suppliers = await this.supplierRepository.GetAll()
            .ToListAsync();
        var result = this.mapper.Map<IEnumerable<SupplierResultDto>>(suppliers);
        return result;
    }
}
