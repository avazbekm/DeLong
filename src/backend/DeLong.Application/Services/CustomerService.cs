using AutoMapper;
using DeLong.Application.DTOs.Customers;
using DeLong.Application.Exceptions;
using DeLong.Application.Extensions;
using DeLong.Application.Interfaces;
using DeLong.Domain.Configurations;
using DeLong.Domain.Entities;
using DeLong.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeLong.Service.Services;

#pragma warning disable // warninglarni o'chirish uchun
public class CustomerService : ICustomerService
{
    private readonly IMapper mapper;
    private readonly IRepository<Customer> customerRepository;
    public CustomerService(IRepository<Customer> customerRepository, IMapper mapper)
    {
        this.mapper = mapper;
        this.customerRepository = customerRepository;
    }

    public async ValueTask<CustomerResultDto> AddAsync(CustomerCreationDto dto)
    {
        if (dto.JSHSHIR.Equals(""))
        {
            Customer existCustomer = await this.customerRepository.GetAsync(u => u.INN.Equals(dto.INN));
            if (existCustomer is not null)
                throw new AlreadyExistException($"This customer is already exists with INN = {dto.INN}");
        }
        else if (dto.INN.Equals(0))
        {
            Customer existCustomer = await this.customerRepository.GetAsync(u => u.JSHSHIR.Equals(dto.JSHSHIR));
            if (existCustomer is not null)
                throw new AlreadyExistException($"This customer is already exists with JSHSHIR = {dto.JSHSHIR}");
        }

        var mappedCustomer = this.mapper.Map<Customer>(dto);
        await this.customerRepository.CreateAsync(mappedCustomer);
        await this.customerRepository.SaveChanges();

        var result = this.mapper.Map<CustomerResultDto>(mappedCustomer);
        return result;
    }

    public async ValueTask<CustomerResultDto> ModifyAsync(CustomerUpdateDto dto)
    {
        Customer existCustomer = await this.customerRepository.GetAsync(u => u.Id.Equals(dto.Id))
            ?? throw new NotFoundException($"This customer is not found with ID = {dto.Id}");

        var mappedCustomer = this.mapper.Map(dto, existCustomer);
        this.customerRepository.Update(mappedCustomer);
        await this.customerRepository.SaveChanges();

        var result = this.mapper.Map<CustomerResultDto>(mappedCustomer);
        return result;
    }

    public async ValueTask<bool> RemoveAsync(long id)
    {
        Customer existCustomer = await this.customerRepository.GetAsync(u => u.Id.Equals(id))
            ?? throw new NotFoundException($"This customer is not found with ID = {id}");

        this.customerRepository.Delete(existCustomer);
        await this.customerRepository.SaveChanges();
        return true;
    }

    public async ValueTask<CustomerResultDto> RetrieveByIdAsync(long id)
    {
        Customer existCustomer = await this.customerRepository.GetAsync(u => u.Id.Equals(id))
            ?? throw new NotFoundException($"This customer is not found with ID = {id}");

        var result = this.mapper.Map<CustomerResultDto>(existCustomer);
        return result;
    }

    public async ValueTask<IEnumerable<CustomerResultDto>> RetrieveAllAsync(PaginationParams @params, Filter filter, string search = null)
    {
        var customers = await this.customerRepository.GetAll()
            .ToPaginate(@params)
            .OrderBy(filter)
            .ToListAsync();

        var result = customers.Where(customer => customer.Name.Contains(search, StringComparison.OrdinalIgnoreCase));
        var mappedCustomers = this.mapper.Map<List<CustomerResultDto>>(result);
        return mappedCustomers;
    }

    public async ValueTask<IEnumerable<CustomerResultDto>> RetrieveAllAsync()
    {
        var customers = await this.customerRepository.GetAll()
            .ToListAsync();
        var result = this.mapper.Map<IEnumerable<CustomerResultDto>>(customers);
        return result;
    }

    public async ValueTask<CustomerResultDto> RetrieveByInnAsync(int INN)
    {
        Customer existCustomer = await this.customerRepository.GetAsync(customer => customer.INN.Equals(INN))
            ?? throw new NotFoundException($"This customer is not found with phone = {INN}");

        var result = this.mapper.Map<CustomerResultDto>(existCustomer);
        return result;
    }

    public async ValueTask<CustomerResultDto> RetrieveByJshshirAsync(string jshshir)
    {
        Customer existCustomer = await this.customerRepository.GetAsync(customer => customer.JSHSHIR.Equals(jshshir))
            ?? throw new NotFoundException($"This customer is not found with JSHSHIR = {jshshir}");

        var result = this.mapper.Map<CustomerResultDto>(existCustomer);
        return result;
    }
}
