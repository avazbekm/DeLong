using AutoMapper;
using DeLong.Domain.Entities;
using DeLong.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using DeLong.Domain.Configurations;
using DeLong.Application.Exceptions;
using DeLong.Application.Extensions;
using DeLong.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using DeLong.Application.DTOs.Customers;

namespace DeLong.Service.Services;

#pragma warning disable // warninglarni o'chirish uchun
public class CustomerService : AuditableService, ICustomerService
{
    private readonly IMapper _mapper;
    private readonly IRepository<Customer> _customerRepository;

    public CustomerService(IRepository<Customer> customerRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        : base(httpContextAccessor)
    {
        _mapper = mapper;
        _customerRepository = customerRepository;
    }

    public async ValueTask<CustomerResultDto> AddAsync(CustomerCreationDto dto)
    {
        if (string.IsNullOrEmpty(dto.JSHSHIR))
        {
            Customer existCustomer = await _customerRepository.GetAsync(u => u.INN.Equals(dto.INN) && !u.IsDeleted);
            if (existCustomer is not null)
                throw new AlreadyExistException($"This customer is already exists with INN = {dto.INN}");
        }
        else if (dto.INN == 0 || !dto.INN.HasValue)
        {
            Customer existCustomer = await _customerRepository.GetAsync(u => u.JSHSHIR.Equals(dto.JSHSHIR) && !u.IsDeleted);
            if (existCustomer is not null)
                throw new AlreadyExistException($"This customer is already exists with JSHSHIR = {dto.JSHSHIR}");
        }

        var mappedCustomer = _mapper.Map<Customer>(dto);
        SetCreatedFields(mappedCustomer); // Auditable maydonlarni qo‘shish
        mappedCustomer.BranchId = GetCurrentBranchId();
        await _customerRepository.CreateAsync(mappedCustomer);
        await _customerRepository.SaveChanges();

        return _mapper.Map<CustomerResultDto>(mappedCustomer);
    }

    public async ValueTask<CustomerResultDto> ModifyAsync(CustomerUpdateDto dto)
    {
        Customer existCustomer = await _customerRepository.GetAsync(u => u.Id.Equals(dto.Id) && !u.IsDeleted)
            ?? throw new NotFoundException($"This customer is not found with ID = {dto.Id}");

        _mapper.Map(dto, existCustomer);
        SetUpdatedFields(existCustomer); // Auditable maydonlarni yangilash

        _customerRepository.Update(existCustomer);
        await _customerRepository.SaveChanges();

        return _mapper.Map<CustomerResultDto>(existCustomer);
    }

    public async ValueTask<bool> RemoveAsync(long id)
    {
        Customer existCustomer = await _customerRepository.GetAsync(u => u.Id.Equals(id) && !u.IsDeleted)
            ?? throw new NotFoundException($"This customer is not found with ID = {id}");

        existCustomer.IsDeleted = true; // Soft delete
        SetUpdatedFields(existCustomer); // Auditable maydonlarni yangilash

        _customerRepository.Update(existCustomer); // Delete o‘rniga Update
        await _customerRepository.SaveChanges();
        return true;
    }

    public async ValueTask<CustomerResultDto> RetrieveByIdAsync(long id)
    {
        var branchId = GetCurrentBranchId();
        Customer existCustomer = await _customerRepository.GetAsync(u => u.Id.Equals(id) && !u.IsDeleted && u.BranchId.Equals(branchId))
            ?? throw new NotFoundException($"This customer is not found with ID = {id}");

        return _mapper.Map<CustomerResultDto>(existCustomer);
    }

    public async ValueTask<IEnumerable<CustomerResultDto>> RetrieveAllAsync(PaginationParams @params, Filter filter, string search = null)
    {
        var branchId = GetCurrentBranchId();
        var customersQuery = _customerRepository.GetAll(u => !u.IsDeleted && u.BranchId.Equals(branchId))
            .ToPaginate(@params)
            .OrderBy(filter);

        if (!string.IsNullOrEmpty(search))
        {
            customersQuery = customersQuery.Where(customer => customer.Name.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        var customers = await customersQuery.ToListAsync();
        return _mapper.Map<List<CustomerResultDto>>(customers);
    }

    public async ValueTask<IEnumerable<CustomerResultDto>> RetrieveAllAsync()
    {
        var branchId = GetCurrentBranchId();
        var customers = await _customerRepository.GetAll(u => !u.IsDeleted && u.BranchId.Equals(branchId))
            .ToListAsync();
        return _mapper.Map<List<CustomerResultDto>>(customers);
    }

    public async ValueTask<CustomerResultDto> RetrieveByInnAsync(int INN)
    {
        var branchId = GetCurrentBranchId();
        Customer existCustomer = await _customerRepository.GetAsync(customer => customer.INN.Equals(INN) && !customer.IsDeleted && customer.BranchId.Equals(branchId))
            ?? throw new NotFoundException($"This customer is not found with INN = {INN}");

        return _mapper.Map<CustomerResultDto>(existCustomer);
    }

    public async ValueTask<CustomerResultDto> RetrieveByJshshirAsync(string jshshir)
    {
        var branchId = GetCurrentBranchId();
        Customer existCustomer = await _customerRepository.GetAsync(customer => customer.JSHSHIR.Equals(jshshir) && !customer.IsDeleted && customer.BranchId.Equals(branchId))
            ?? throw new NotFoundException($"This customer is not found with JSHSHIR = {jshshir}");

        return _mapper.Map<CustomerResultDto>(existCustomer);
    }
}